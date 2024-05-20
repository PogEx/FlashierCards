namespace Backend.RestApi.Contracts.Content;

public interface IFolderHandler
{
    Task<Guid> CreateFolder(Guid owner, Guid parent, string name);
    Task CreateUserRoot(Guid owner);
    Task DeleteFolder(Guid folder);
    Task ChangeFolder(string name);
    Task<IEnumerable<Guid>> GetChildren(Guid folder);
    Task<Guid> GetParentFolder(Guid folder);
    Task<Guid> GetUserRoot(Guid owner);
}