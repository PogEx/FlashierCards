using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.RestApi.Controllers;

[Route("card")]
[Controller]
public class CardController : Controller
{
    // GET
    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}