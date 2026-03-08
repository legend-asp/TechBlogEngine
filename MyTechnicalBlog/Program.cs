using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using NLog;
using NLog.Web;

// 1. Инициализация NLog логгера для захвата ошибок старта
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 2. Подключаем NLog как провайдер логирования
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddControllersWithViews();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    // --- ГЛОБАЛЬНЫЙ ОБРАБОТЧИК ИСКЛЮЧЕНИЙ ---
    if (!app.Environment.IsDevelopment())
    {
        // В продакшене перенаправляем на красивую страницу ошибки
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        // В разработке показываем детальную страницу ошибки для дебага
        app.UseDeveloperExceptionPage();
    }

    // Обработка статусных кодов (404, 403 и т.д.)
    app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");
    // ----------------------------------------

    // Логируем запуск приложения
    app.Logger.LogInformation("Приложение MyTechnicalBlog успешно запущено.");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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