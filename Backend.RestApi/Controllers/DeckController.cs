using Backend.Common.Models.Decks;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("deck")]
[Controller]
[Authorize]
public class DeckController(IDeckHandler deckHandler) : Controller
{
    // GET
    [HttpGet]
    public async Task<IActionResult> GetDeck(
        [FromQuery(Name = "id"), BindRequired] Guid id
        )
    {
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeck(
        [FromBody, BindRequired] DeckCreateData data
        )
    {
        await deckHandler.CreateDeck(
            User.GetCurrentUser(), 
            data);
        return Created();
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDeck(
        [FromQuery(Name = "id"), BindRequired] Guid id, 
        [FromBody, BindRequired] DeckChangeData data)
    {
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteDeck(
        [FromQuery(Name = "id"), BindRequired] Guid id)
    {
        Result result = await deckHandler.DeleteDeck(User.GetCurrentUser(), id);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    [HttpPost("import")]
    public IActionResult ImportDeck([FromBody] Guid id)
    {
        return Created();
    }
}