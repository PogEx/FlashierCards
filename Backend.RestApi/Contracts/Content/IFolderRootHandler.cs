using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface IFolderRootHandler
{
    Task<Result> CreateUserRoot(Guid caller);
}