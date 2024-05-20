using Backend.Common.Models.Decks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("deck")]
[Controller]
[Authorize]
public class DeckController : Controller
{
    // GET
    [HttpGet]
    public IActionResult GetDeck(
        [FromQuery(Name = "id"), BindRequired] string id
        )
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateDeck(
        [FromBody, BindRequired] DeckCreateData data
        )
    {
        return Created();
    }

    [HttpPatch]
    public IActionResult ChangeDeck(
        [FromQuery(Name = "id"), BindRequired] string id, 
        [FromBody, BindRequired] DeckChangeData data)
    {
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult DeleteDeck(
        [FromQuery(Name = "id"), BindRequired] string id)
    {
        return Ok();
    }

    [HttpPost("import")]
    public IActionResult ImportDeck([FromBody] string id)
    {
        return Created();
    }
}