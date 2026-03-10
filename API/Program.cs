using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Контроллеры + защита от циклов в JSON (чтобы Swagger не выдавал 500)
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// 2. Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. База данных (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 4. Регистрация сервисов для внедрения зависимостей (DI)
builder.Services.AddScoped<IArticleService, ArticleService>();

var app = builder.Build();

// Настройка Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Доступно по адресу /swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Маппинг маршрутов API
app.MapControllers();

app.Run();