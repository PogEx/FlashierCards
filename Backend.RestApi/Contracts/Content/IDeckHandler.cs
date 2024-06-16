using Backend.Common.Models.Decks;
using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface IDeckHandler
{
    Task<Result<Guid>> CreateDeck(Guid caller, DeckCreateData data);
    Task<Result<IEnumerable<DeckDto>>> GetDecksFromFolder(Guid caller, Guid folderId);
    Task<Result<DeckDto>> GetDeckById(Guid caller, Guid deckId);
    Task<Result> UpdateDeck(Guid caller, Guid deckId, DeckChangeData data);
    Task<Result> DeleteDeck(Guid caller, Guid deckId);

}