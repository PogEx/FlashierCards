using Backend.Common.Models.DatabaseModels;
using Backend.Common.Models.Folders;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Database;
using Backend.RestApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class FolderHandler(Func<FlashiercardsContext> createContext): IFolderHandler
{
    public async Task<Guid> CreateFolder(Guid owner, Guid parent, string name)
    {
        await using (FlashiercardsContext context = createContext())
        {
                Guid folderGuid = Guid.NewGuid();
                await context.Folders.AddAsync(new DbFolder
                {
                    FolderId = folderGuid, 
                    Name = name, 
                    UserId = owner, 
                    IsRoot = false, 
                    ParentId = parent, 
                    ColorHex = ColorHelper.NewRandom().ToHex()
                });
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
                .FirstAsync(f => f.FolderId == folder))
                .Children.Select(child => child.FolderId);
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
            return (await context.Folders
                .FirstAsync(f => f.UserId == user && f.IsRoot == true))
                .FolderId;
        }
    }

    public async Task<Folder> GetFolder(Guid guid)
    {
        await using (FlashiercardsContext context = createContext())
        {
            DbFolder folder = await context.Folders
                .AsNoTracking()
                .Include(dbFolder => dbFolder.Children)
                .FirstAsync(dbFolder => dbFolder.FolderId == guid);
            return new Folder(folder);
        }
    }

    public async Task<Guid> CreateUserRoot(Guid owner)
    {
        await using (FlashiercardsContext context = createContext())
        {
            Guid folderGuid = Guid.NewGuid();
            await context.Folders.AddAsync(new DbFolder
            {
                FolderId = folderGuid, 
                Name = "Home", 
                UserId = owner, 
                IsRoot = true, 
                ParentId = null, 
                ColorHex = "000000"
            });
            await context.SaveChangesAsync();
            return folderGuid;
        }
    }

    public async Task<bool> DeleteFolder(Guid id)
    {
        await using (FlashiercardsContext context = createContext())
        {
            DbFolder dbFolder = await context.Folders
                .Include(f => f.Children)
                .FirstAsync(f => f.FolderId == id);
            if (dbFolder.IsRoot)
                return false;
            
            context.RemoveRange(dbFolder.Children);
            context.Remove(dbFolder);
            await context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<bool> ChangeFolder(Guid folder, string? newName = null, 
        Guid? newParent = null)
    {
        await using (FlashiercardsContext context = createContext())
        {
            DbFolder dbFolderToChange = await context.Folders
                .FirstAsync(f => f.FolderId == folder);
            
            dbFolderToChange.ParentId = newParent ?? dbFolderToChange.ParentId;
            dbFolderToChange.Name = newName ?? dbFolderToChange.Name;
            
            await context.SaveChangesAsync();
            return true;
        }
    }
}