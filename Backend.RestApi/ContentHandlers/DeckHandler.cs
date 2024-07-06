using Backend.Common.Extensions;
using Backend.Common.Models.Decks;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using Backend.RestApi.Logging.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class DeckHandler(FlashiercardsContext context): IDeckHandler, IShareable<string>
{
    public async Task<Result<Guid>> CreateDeck(Guid caller, DeckCreateData data)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(data.Name)) 
                return Result.Fail(new BadRequestError("The supplied name cannot be empty"));

            if (data.Folder == Guid.Empty)
                return Result.Fail(new BadRequestError("No containing folder supplied"));
            
            Guid guid = Guid.NewGuid();

            User owningUser = await context.Users
                .FirstAsync(user => user.UserId == caller);

            Folder containingFolder = await context.Folders
                .FirstAsync(folder => folder.FolderId == data.Folder);

            await context.Decks.AddAsync(new Deck
            {
                DeckId = guid,
                DeckTitle = data.Name,
                User = owningUser,
                Folder = containingFolder
            });

            await context.SaveChangesAsync();
            return guid;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
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
            .Include(d => d.Cards)
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
            Deck deckToRemove = await context.Decks
                .Include(d => d.User)
                .FirstAsync(d => d.DeckId == deckId);
            
            if (deckToRemove.UserId != caller) return Result.Fail(new ForbiddenError(caller, deckId));

            context.Decks.Remove(deckToRemove);
            
            await context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }

    public async Task<Result<string>> Share(Guid caller, Guid id, int duration)
    {
        try
        {
            Deck deck = await context.Decks
                .Include(d => d.DeckInviteCode)
                .FirstAsync(d => d.DeckId == id);

            
            if (deck.DeckInviteCode is null)
            {
                string inviteCode = await GenerateInviteCode();
                await context.DeckInviteCodes.AddAsync(new()
                {
                    DeckId = deck.DeckId, 
                    Code = inviteCode, 
                    ExpiryTime = DateTime.Now.AddMinutes(duration), 
                    Deck = deck
                });
                await context.SaveChangesAsync();
                return inviteCode;
            }

            deck.DeckInviteCode.ExpiryTime = DateTime.Now.AddMinutes(duration);
            context.DeckInviteCodes.Update(deck.DeckInviteCode);
            await context.SaveChangesAsync();
            return deck.DeckInviteCode.Code;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }

    public async Task<Result<Guid>> Import(Guid caller, Guid folderId, string code)
    {
        DeckInviteCode inviteCode = await context.DeckInviteCodes
            .Include(ic => ic.Deck)
            .ThenInclude(d => d.Cards)
            .FirstAsync(ic => ic.Code == code);
        User user = await context.Users
            .Include(u => u.Folders)
            .FirstAsync(u => u.UserId == caller);
        Folder f = folderId == Guid.Empty ? 
            user.Folders.First(f => f.IsRoot) : 
            user.Folders.First(f => f.FolderId == folderId);

        Deck d = inviteCode.Deck.Clone();
        d.UserId = caller;
        d.Cards.ForEach(c => c.UserId = caller);
        d.FolderId = f.FolderId;
        await context.Decks.AddAsync(d);
        
        await context.SaveChangesAsync();
        return d.DeckId;
    }
    
    private static Task<string> GenerateInviteCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new();
        return Task.FromResult(new string(Enumerable.Repeat(chars, 5)
            .Select(s => s[random.Next(s.Length)]).ToArray()));
    }
}