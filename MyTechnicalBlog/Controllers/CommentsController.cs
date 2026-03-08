using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Models; 
using Microsoft.Extensions.Logging;

namespace MyTechnicalBlog.Controllers;

public class RolesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RolesController> _logger;

    public RolesController(ApplicationDbContext context, ILogger<RolesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        // Теперь ToListAsync подсвечиваться не будет
        var roles = await _context.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Role role)
    {
        if (ModelState.IsValid)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Создана новая роль в системе: {RoleName}", role.Name);
            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Попытка создания некорректной роли.");
        var roles = await _context.Roles.ToListAsync();
        return View("Index", roles);
    }
}