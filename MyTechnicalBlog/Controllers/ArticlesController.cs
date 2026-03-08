using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Models;

namespace MyTechnicalBlog.Controllers;

public class ArticlesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(ApplicationDbContext context, ILogger<ArticlesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _context.Articles.Include(s => s.Author).ToListAsync();
        return View(articles);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Article article)
    {
        // 1. Убираем навигационные свойства из валидации, иначе ModelState всегда будет False
        ModelState.Remove("Author");
        ModelState.Remove("Tags");
        ModelState.Remove("Comments");

        if (ModelState.IsValid)
        {
            try
            {
                // 2. Ищем пользователя 'AdminUser', которого ты создал в DbContext
                var defaultAuthor = await _context.Users.FirstOrDefaultAsync(u => u.Username == "AdminUser");

                if (defaultAuthor != null)
                {
                    article.AuthorId = defaultAuthor.Id;
                    // Важно: обнуляем сам объект, чтобы EF не пытался создать нового юзера
                    article.Author = null;
                }
                else
                {
                    // Если вдруг админа нет, берем вообще любого первого юзера
                    var anyUser = await _context.Users.FirstOrDefaultAsync();
                    if (anyUser != null) article.AuthorId = anyUser.Id;
                }

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Статья '{Title}' успешно создана.", article.Title);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении статьи в базу данных.");
                ModelState.AddModelError("", "Ошибка БД: " + ex.Message);
            }
        }

        // Если дошли сюда — значит что-то не так, возвращаем список, чтобы страница не упала
        var articles = await _context.Articles.Include(s => s.Author).ToListAsync();
        return View("Index", articles);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            string title = article.Title;
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Статья удалена: {Title}", title);
        }
        else
        {
            _logger.LogWarning("Попытка удаления несуществующей статьи с ID: {Id}", id);
        }
        return RedirectToAction(nameof(Index));
    }
}