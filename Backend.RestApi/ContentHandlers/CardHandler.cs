using Backend.Common.Models.Cards;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using Backend.RestApi.Logging.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class CardHandler(FlashiercardsContext context): ICardHandler
{
    public async Task<Result<Guid>> CreateCard(Guid caller, CardCreateData data)
    {
        try
        {
            Deck deck = await context.Decks.FirstAsync(deck => deck.DeckId == data.DeckId);
            if (deck.UserId != caller) return Result.Fail(new ForbiddenError(caller, data.DeckId));
            
            Guid guid = Guid.NewGuid();
            if (data.BackId is not null)
            {
                Card back = await context.Cards.FirstAsync(card => card.CardId == data.BackId);
                
                if (back.DeckId != data.DeckId) 
                    return Result.Fail(new BadRequestError("Back card has to be in the same deck as front"));
                if (back.BackId is not null) 
                    return Result.Fail(new BadRequestError("Can't add back to card that already has a backside"));
                
                back.BackId = guid;
            }
            
            CardType type =
                await context.CardTypes.FirstOrDefaultAsync(type =>
                    type.Type.ToLower() == data.ContentType.ToLower()) ??
                new CardType { Id = 1, Type = "empty" };
            
            await context.Cards.AddAsync(new Card
            {
                CardId = guid,
                UserId = caller,
                DeckId = data.DeckId,
                BackId = data.BackId,
                Text = data.Text,
                TypeNavigation = type
            });
            await context.SaveChangesAsync();
            return guid;
        }
        catch (DbUpdateException e)
        {
            return Result.Fail(new DatabaseError(e));
        }
    }

    public async Task<Result<CardDto>> GetCardById(Guid caller, Guid cardId)
    {
        try
        {
            return (await context.Cards.FirstAsync(card => card.CardId == cardId && card.UserId == caller)).ToDto();
        }
        catch (DbUpdateException e)
        {
            return Result.Fail(new DatabaseError(e));
        }
    }

    public async Task<Result> UpdateCard(Guid caller, Guid cardId, CardChangeData data)
    {
        try
        {
            Card card = await context.Cards.FirstAsync(card => card.CardId == cardId && card.UserId == caller);
            CardType? type = await context.CardTypes.FirstOrDefaultAsync(c => c.Type == data.CardType);
            card.Text = data.Text ?? card.Text;
            card.Type = type?.Id ?? card.Type;
            
            await context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return Result.Fail(new DatabaseError(e));
        }
    }

    public async Task<Result> DeleteCard(Guid caller, Guid cardId)
    {
        try
        {
            Card card = await context.Cards.FirstAsync(card => card.CardId == cardId && card.UserId == caller);
            context.Cards.Remove(card);
            await context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return Result.Fail(new DatabaseError(e));
        }
    }
}