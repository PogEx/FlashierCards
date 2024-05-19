using System.Data;
using Backend.RestApi.Contracts.Content;
using Dapper;
using Dapper.SimpleSqlBuilder;

namespace Backend.RestApi.ContentHandlers;

public class FolderHandler(IDbConnection connection): IFolderHandler
{
    public async Task<Guid> CreateFolder(Guid owner, Guid parent, string name)
    {
        Guid createdFolder = await CreateFolder(owner, name, false);
        
        Builder builder = SimpleBuilder.Create($"INSERT INTO folder_parent_child_link(parent_folder_id, child_folder_id) VALUES ({parent}, {createdFolder})");
        await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        
        return createdFolder;
    }

    public async Task<IEnumerable<Guid>> GetChildren(Guid folder)
    {    
        Builder builder = SimpleBuilder.Create($"SELECT child_folder_id FROM folder_parent_child_link WHERE parent_folder_id = {folder} ");
        return await connection.QueryAsync<Guid>(builder.Sql, builder.Parameters);
    }
    
    public async Task<Guid> GetParentFolder(Guid folder)
    {
        Builder builder = SimpleBuilder.Create($"SELECT parent_folder_id FROM folder_parent_child_link WHERE child_folder_id = {folder} ");
        return await connection.QueryFirstAsync<Guid>(builder.Sql, builder.Parameters);
    }
    
    public async Task<Guid> GetUserRoot(Guid user)
    {
        Builder builder = SimpleBuilder.Create($"SELECT folder_id FROM folder WHERE user_id = {user.ToString()} AND is_root = true");
        return await connection.QueryFirstAsync<Guid>(builder.Sql, builder.Parameters);
    }
    
    public async Task CreateUserRoot(Guid owner)
    {
        await CreateFolder(owner, "Home", true);
    }

    public async Task DeleteFolder(Guid folder)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeFolder(string name)
    {
        throw new NotImplementedException();
    }

    private async Task<Guid> CreateFolder(Guid owner, string name, bool isRoot)
    {
        Guid folderGuid = Guid.NewGuid();
        Builder builder = SimpleBuilder.Create($"INSERT INTO folder(folder_id, user_id, is_root, name) VALUES ({folderGuid}, {owner}, {isRoot}, {name})");
        await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        return folderGuid;
    }
}