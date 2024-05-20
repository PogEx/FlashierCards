using Backend.Common.Models;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Database;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class FolderHandler(Func<FlashiercardsContext> createContext): IFolderHandler
{
    public async Task<Guid> CreateFolder(Guid owner, Guid parent, string name)
    {
        await using (FlashiercardsContext context = createContext())
        {
                Guid folderGuid = Guid.NewGuid();
                await context.Folders.AddAsync(new Folder
                    { FolderId = folderGuid, Name = name, UserId = owner, IsRoot = false, ParentId = parent});
                await context.SaveChangesAsync();
                return folderGuid;
        }
    }

    public async Task<IEnumerable<Guid>> GetChildren(Guid folder)
    {    
        await using (FlashiercardsContext context = createContext())
        {
            return (await context.Folders
                .Include(f => f.Children)
                .FirstAsync(f => f.FolderId == folder)).Children.Select(child => child.FolderId);
        }
    }
    
    public async Task<Guid?> GetParentFolder(Guid folder)
    {
        await using (FlashiercardsContext context = createContext())
        {
            return (await context.Folders
                    .FirstAsync(f => f.FolderId == folder)).ParentId;
        }
    }
    
    public async Task<Guid> GetUserRoot(Guid user)
    {
        await using (FlashiercardsContext context = createContext())
        {
            return (await context.Folders.FirstAsync(f => f.UserId == user && f.IsRoot == true)).FolderId;
        }
    }
    
    public async Task<Guid> CreateUserRoot(Guid owner)
    {
        await using (FlashiercardsContext context = createContext())
        {
            Guid folderGuid = Guid.NewGuid();
            await context.Folders.AddAsync(new Folder
                { FolderId = folderGuid, Name = "Home", UserId = owner, IsRoot = true, ParentId = null});
            await context.SaveChangesAsync();
            return folderGuid;
        }
    }

    public async Task<bool> DeleteFolder(Guid id)
    {
        await using (FlashiercardsContext context = createContext())
        {
            Folder folder = await context.Folders
                .Include(f => f.Children)
                .FirstAsync(f => f.FolderId == id);
            if (folder.IsRoot)
                return false;
            
            context.RemoveRange(folder.Children);
            context.Remove(folder);
            await context.SaveChangesAsync();
            return true;
        }
    }

    public async Task ChangeFolder(string name)
    {
        throw new NotImplementedException();
    }
}