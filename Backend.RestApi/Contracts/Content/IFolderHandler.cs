using Backend.Common.Models.Folders;
using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface IFolderHandler
{
    Task<Result<Guid>> CreateFolder(Guid caller, FolderCreateData data);
    Task<Result<Guid>> CreateUserRoot(Guid caller);
    Task<Result> DeleteFolder(Guid caller, Guid folderId);
    Task<Result> ChangeFolder(Guid caller, Guid folder, FolderChangeData data);
    Task<Result<Guid>> GetUserRoot(Guid caller);
    Task<Result<FolderDto>> GetFolder(Guid caller, Guid guid);
}