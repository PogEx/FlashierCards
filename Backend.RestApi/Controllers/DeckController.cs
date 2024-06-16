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
public class DeckController(IDeckHandler deckHandler, IShareable<string> sharable) : Controller
{
    // GET
    [HttpGet]
    public async Task<IActionResult> GetDeck(
        [FromQuery(Name = "id"), BindRequired] Guid id
        )
    {
        Result<DeckDto> result = await deckHandler.GetDeckById(User.GetCurrentUser(), id);
        return Ok(result.Value);
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
        Result result = await deckHandler.UpdateDeck(User.GetCurrentUser(), id, data);
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
    public async Task<IActionResult> ImportDeck(
        [FromQuery(Name="key"), BindRequired] string key,
        [FromQuery(Name="folder")] Guid folderId
        )
    {
        Result<Guid> result = await sharable.Import(User.GetCurrentUser(), folderId, key);
        return Ok(result.Value);
    }
    
    [HttpPost("share")]
    public async Task<IActionResult> ShareDeck(
        [FromQuery(Name="id"), BindRequired] Guid id, 
        [FromQuery(Name="duration")] int duration = 5
        )
    {
        Result<string> result = await sharable.Share(User.GetCurrentUser(), id, duration);
        return Ok(result.Value);
    }
}