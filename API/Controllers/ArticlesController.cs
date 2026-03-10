using Microsoft.AspNetCore.Mvc;
using MyTechnicalBlog.Models;
using MyTechnicalBlog.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/Articles
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Article>), 200)]
        public async Task<IActionResult> Get()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        // POST: api/Articles
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _articleService.CreateArticleAsync(article);

            if (!result)
                return StatusCode(500, "Ошибка при сохранении статьи.");

            return Ok();
        }
    }
}