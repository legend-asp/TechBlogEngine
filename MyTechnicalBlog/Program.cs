using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Services;
using NLog;
using NLog.Web;
using Newtonsoft.Json; // Не забудь добавить using

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddScoped<IArticleService, ArticleService>();

    // ИСПРАВЛЕНИЕ 1: Игнорируем циклы в JSON (чтобы Swagger не падал на Article -> User -> Article)
    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    builder.Services.AddEndpointsApiExplorer();

    // ИСПРАВЛЕНИЕ 2: Лечим конфликт имен классов (SchemaId conflict)
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My Blog API", Version = "v1" });
        c.CustomSchemaIds(type => type.FullName);
    });

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => { options.LoginPath = "/Account/Login"; });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Программа остановлена из-за исключения при запуске");
    throw;
}
finally
{
    LogManager.Shutdown();
}