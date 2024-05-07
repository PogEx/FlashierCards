using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.RestApi.Controllers;

[Route("deck")]
[Controller]
public class DeckController : Controller
{
    // GET
    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}