using Backend.Common.Models.Cards;
using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface ICardHandler
{
    Task<Result<Guid>> CreateCard(Guid caller, CardCreateData data);
    Task<Result<CardDto>> GetCardById(Guid caller, Guid cardId);
    Task<Result> UpdateCard(Guid caller, Guid cardId, CardChangeData data);
    Task<Result> DeleteCard(Guid caller, Guid cardId);
}