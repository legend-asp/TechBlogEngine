using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Data;
using MyTechnicalBlog.Models;

namespace MyTechnicalBlog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(ApplicationDbContext context, ILogger<ArticleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.Include(a => a.Author).ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> CreateArticleAsync(Article article)
        {
            try
            {
                // Логика привязки к AdminUser
                var defaultAuthor = await _context.Users.FirstOrDefaultAsync(u => u.Username == "AdminUser");
                if (defaultAuthor != null)
                {
                    article.AuthorId = defaultAuthor.Id;
                    article.Author = null;
                }
                else
                {
                    var anyUser = await _context.Users.FirstOrDefaultAsync();
                    if (anyUser != null) article.AuthorId = anyUser.Id;
                }

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании статьи в сервисе.");
                return false;
            }
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}