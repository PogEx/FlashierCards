using Backend.Common.Models.Cards;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("card")]
[Controller]
[Authorize]
public class CardController (ICardHandler cardHandler) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetCard([FromQuery(Name = "id"), BindRequired] Guid id)
    {
        Result<CardDto> card = await cardHandler.GetCardById(User.GetCurrentUser(), id);
        return Ok(card.Value);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody, BindRequired] CardCreateData data)
    {
        await cardHandler.CreateCard(User.GetCurrentUser(), data);
        return Ok();
    }
    [HttpPatch]
    public async Task<IActionResult>  ChangeCard(
        [FromForm(Name = "id"), BindRequired] Guid id,
        [FromBody, BindRequired] CardChangeData data)
    {
        return Ok();
    }
    [HttpDelete]
    public async Task<IActionResult>  RemoveCard()
    {
        return Ok();
    }
}