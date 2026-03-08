using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();

    public IActionResult Error()
    {
        _logger.LogError("Пользователь столкнулся с внутренней ошибкой сервера (500).");
        return View();
    }

    [Route("Home/HandleError/{code}")]
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