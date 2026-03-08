using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Models;
using Microsoft.Extensions.Logging;

namespace MyTechnicalBlog.Controllers;

public class TagsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ApplicationDbContext context, ILogger<TagsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var tags = await _context.Tags.ToListAsync();
        return View(tags);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Create(Tag tag)
    {
        // Принудительно отключаем валидацию связей, которых нет в форме
        ModelState.Remove("Articles");

        if (ModelState.IsValid)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Создан новый тег: {TagName}", tag.Name);
            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Неудачная попытка создания тега.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null)
        {
            string tagName = tag.Name;
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Тег удален: {TagName}", tagName);
        }
        else
        {
            _logger.LogWarning("Попытка удаления несуществующего тега с ID: {Id}", id);
        }
        return RedirectToAction(nameof(Index));
    }
}