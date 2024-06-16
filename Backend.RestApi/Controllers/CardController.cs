using Backend.Common.Models.Cards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("card")]
[Controller]
[Authorize]
public class CardController : Controller
{
    [HttpGet]
    public IActionResult GetCard([FromQuery(Name = "id"), BindRequired] Guid id)
    {
        return Ok();
    }
    [HttpPost]
    public IActionResult CreateCard([FromBody, BindRequired] CardCreateData data)
    {
        return Ok();
    }
    [HttpPatch]
    public IActionResult ChangeCard(
        [FromForm(Name = "id"), BindRequired] Guid id,
        [FromBody, BindRequired] CardChangeData data)
    {
        return Ok();
    }
    [HttpDelete]
    public IActionResult RemoveCard()
    {
        return Ok();
    }
}