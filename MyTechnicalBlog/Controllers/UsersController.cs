using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using Microsoft.Extensions.Logging;

namespace MyTechnicalBlog.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            string username = user.Username;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Администратор удалил пользователя: {Username} (ID: {Id})", username, id);
        }
        else
        {
            _logger.LogWarning("Попытка удаления несуществующего пользователя с ID: {Id}", id);
        }
        return RedirectToAction(nameof(Index));
    }
}