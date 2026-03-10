using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet] // Добавь для порядка
    public IActionResult Index() => View();

    [HttpGet] // Добавь для порядка
    public IActionResult Error()
    {
        _logger.LogError("Пользователь столкнулся с внутренней ошибкой сервера (500).");
        return View();
    }

    // ИСПРАВЛЕНИЕ: Замени [Route] на [HttpGet] с указанием шаблона
    [HttpGet("Home/HandleError/{code}")]
    public IActionResult HandleError(int code)
    {
        if (code == 404)
        {
            _logger.LogWarning("Ошибка 404: пользователь пытался перейти на несуществующую страницу.");
            return View("NotFound");
        }
        if (code == 403)
        {
            _logger.LogWarning("Ошибка 403: попытка несанкционированного доступа.");
            return View("AccessDenied");
        }
        return View("Error");
    }
}