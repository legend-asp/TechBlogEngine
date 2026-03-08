using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyTechnicalBlog.Models;
using Microsoft.Extensions.Logging; // Обязательно добавь этот namespace

namespace MyTechnicalBlog.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountController> _logger; // Поле для логгера

    // Обновленный конструктор с внедрением логгера
    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username)
    {
        // ЛОГИРОВАНИЕ ДЕЙСТВИЯ: Попытка входа
        _logger.LogInformation("Попытка входа пользователя: {Username}", username);

        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            // ЛОГИРОВАНИЕ ОШИБКИ/ПРЕДУПРЕЖДЕНИЯ
            _logger.LogWarning("Неудачный вход: пользователь {Username} не найден.", username);
            ModelState.AddModelError("", "Пользователь не найден");
            return View();
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        // ЛОГИРОВАНИЕ ДЕЙСТВИЯ: Успешный вход
        _logger.LogInformation("Пользователь {Username} успешно вошел в систему.", username);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        var exists = await _context.Users.AnyAsync(u => u.Username == user.Username);
        if (exists)
        {
            // ЛОГИРОВАНИЕ ПРЕДУПРЕЖДЕНИЯ
            _logger.LogWarning("Попытка регистрации дубликата имени пользователя: {Username}", user.Username);
            ModelState.AddModelError("Username", "Это имя уже занято");
            return View(user);
        }

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ЛОГИРОВАНИЕ ДЕЙСТВИЯ: Успешная регистрация
            _logger.LogInformation("Зарегистрирован новый пользователь: {Username}", user.Username);
        }
        catch (Exception ex)
        {
            // ЛОГИРОВАНИЕ ОШИБКИ ПРИ СОХРАНЕНИИ
            _logger.LogError(ex, "Ошибка при сохранении пользователя {Username} в базу данных", user.Username);
            throw;
        }

        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name;
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // ЛОГИРОВАНИЕ ДЕЙСТВИЯ: Выход
        _logger.LogInformation("Пользователь {Username} вышел из системы.", userName);

        return RedirectToAction("Index", "Home");
    }
}