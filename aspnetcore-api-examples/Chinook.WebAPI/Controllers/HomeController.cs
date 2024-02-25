using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
[ApiVersion("1.0")]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    private readonly ILogger<HomeController> _logger = logger;


    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}