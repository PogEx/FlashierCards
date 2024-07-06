using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface IShareable<T>
{
    Task<Result<T>> Share(Guid caller, Guid id, int duration);
    Task<Result<Guid>> Import(Guid caller, Guid folderId, T code);
}