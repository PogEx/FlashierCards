using Backend.Common.Models.Folders;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers;
using Backend.RestApi.Logging.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class FolderHandler(FlashiercardsContext context): IFolderHandler
{
    public async Task<Result<Guid>> CreateFolder(Guid caller, FolderCreateData data)
    {
        try
        {
            Guid folderGuid = Guid.NewGuid();
            await context.Folders.AddAsync(new Folder
            {
                FolderId = folderGuid,
                Name = data.Name,
                UserId = caller,
                IsRoot = false,
                ParentId = data.Parent,
                ColorHex = ColorHelper.NewRandom().ToHex()
            });
            await context.SaveChangesAsync();
            return folderGuid;
        }
        catch (DbUpdateException e)
        {  
            return new DatabaseError(e);
        }
    }
    
    public async Task<Result<Guid>> GetUserRoot(Guid caller)
    {
        return (await context.Folders
                .FirstAsync(f => f.UserId == caller && f.IsRoot == true))
            .FolderId;
    }

    public async Task<Result<FolderDto>> GetFolder(Guid caller, Guid guid)
    {
        Folder folder = await context.Folders
            .AsNoTracking()
            .Include(dbFolder => dbFolder.Children)
            .Include(dbFolder => dbFolder.Decks)
            .FirstAsync(dbFolder => dbFolder.FolderId == guid);
        return folder.ToDto();
    }

    public async Task<Result<Guid>> CreateUserRoot(Guid caller)
    {
        try
        {
            Guid folderGuid = Guid.NewGuid();
            await context.Folders.AddAsync(new Folder
            {
                FolderId = folderGuid,
                Name = "Home",
                UserId = caller,
                IsRoot = true,
                ParentId = null,
                ColorHex = "000000"
            });
            await context.SaveChangesAsync();
            return folderGuid;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }

    public async Task<Result> DeleteFolder(Guid caller, Guid folderId)
    {
        try
        {

            Folder folder = await context.Folders
                .Include(f => f.Children)
                .Include(f => f.Decks)
                .ThenInclude(d => d.Users)
                .Include(f => f.User)
                .FirstAsync(f => f.FolderId == folderId);

            if (folder.UserId != caller)
                return Result.Fail(new ForbiddenError(caller, folderId));

            if (folder.IsRoot)
                return Result.Fail(new ForbiddenError(caller, folderId));

            context.Folders.Remove(folder);
            await context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }

    public async Task<Result> ChangeFolder(Guid caller, Guid folder, FolderChangeData data)
    {
        try
        {
            Folder folderToChange = await context.Folders
                .FirstAsync(f => f.FolderId == folder);

            folderToChange.ParentId = data.Parent ?? folderToChange.ParentId;
            folderToChange.Name = data.Name ?? folderToChange.Name;
            folderToChange.ColorHex = data.ColorHex ?? folderToChange.ColorHex;

            await context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }
}