using FluentResults;

namespace Backend.RestApi.Contracts.DatabaseOperator;

public interface IDeckHandlerInternal
{
    public Task<Result> DeleteDeckInternal(Guid caller, Guid deckId);
}