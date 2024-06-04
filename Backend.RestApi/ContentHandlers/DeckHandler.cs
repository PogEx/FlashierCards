using Backend.Common.Extensions;
using Backend.Common.Models.Decks;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Contracts.DatabaseOperator;
using Backend.RestApi.Helpers;
using Backend.RestApi.Logging.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class DeckHandler(FlashiercardsContext context): IDeckHandler, IDeckHandlerInternal
{
    public async Task<Result<Guid>> CreateDeck(Guid caller, DeckCreateData data)
    {
        try
        {
            Guid guid = Guid.NewGuid();

            User owningUser = await context.Users
                .FirstAsync(user => user.UserId == caller);

            Folder containingFolder = await context.Folders
                .FirstAsync(folder => folder.FolderId == data.Folder);

            await context.Decks.AddAsync(new Deck
            {
                DeckId = guid,
                DeckTitle = data.Name,
                Users = new List<User> { owningUser },
                Folders = new List<Folder> { containingFolder }
            });

            await context.SaveChangesAsync();
            return guid;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError().CausedBy(e);
        }
    }

    public async Task<Result<IEnumerable<DeckDto>>> GetDecksFromFolder(Guid caller, Guid folderId)
    {
        return Result.Ok((await context.Folders
                .AsNoTracking()
                .Include(f => f.Decks)
                .FirstAsync(f => f.FolderId == folderId)).Decks
            .MapTo(deck => deck.ToDto()));
    }

    public async Task<Result<DeckDto>> GetDeckById(Guid caller, Guid deckId)
    {
        return (await context.Decks
            .AsNoTracking()
            .FirstAsync(d => d.DeckId == deckId)).ToDto();
    }

    public async Task<Result> UpdateDeck(Guid caller, Guid deckId, DeckChangeData data)
    {
        Deck folderToChange = await context.Decks
            .FirstAsync(f => f.DeckId == deckId);
            
        folderToChange.DeckTitle = data.Name ?? folderToChange.DeckTitle;
            
        await context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteDeck(Guid caller, Guid deckId)
    {
        try
        {
            Result result = await DeleteDeckInternal(caller, deckId);
            
            await context.SaveChangesAsync();
            return result;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError().CausedBy(e);
        }
    }

    public async Task<Result> DeleteDeckInternal(Guid caller, Guid deckId)
    {
        try
        {
            User callingUser = await context.Users
                .FirstAsync(u => u.UserId == caller);
            
            Deck deckToRemove = await context.Decks
                .Include(d => d.Users)
                .FirstAsync(d => d.DeckId == deckId);

            deckToRemove.Users.Remove(callingUser);

            if (deckToRemove.Users.Count == 0)
                context.Decks.Remove(deckToRemove);
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError().CausedBy(e);
        }
    }
}