using Backend.Common.Extensions;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class FolderPage : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Parameter] public string FolderId { get; set; }
    private string Name;
    private List<FolderGhostContainer> folders = new();
    private List<Deck> decks = new();

    protected override async Task OnInitializedAsync()
    {
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            Folder folder;
            if (FolderId is null)
            {
                 folder = await context.Folders
                    .Include(f => f.Children)
                    .Include(f => f.Decks)
                    .FirstAsync(folder => folder.IsRoot == true);
            }
            else
            {
                Guid folderGuid = Guid.Parse(FolderId);
                folder = await context.Folders
                    .Include(f => f.Children)
                    .Include(f => f.Decks)
                    .FirstOrDefaultAsync(folder => folder.FolderId == folderGuid);
            }
            
            FolderId = folder.FolderId.ToString();
            Name = folder.Name;
            folders = folder.Children
                .OrderBy(f => f.Name.Length)
                .ThenBy(f => f.Name)
                .MapTo(f => new FolderGhostContainer{Folder = f})
                .ToList();
            decks = folder.Decks.OrderBy(f => f.DeckTitle.Length).ThenBy(f => f.DeckTitle).ToList();
        }
    }
    
    private async Task CreateFolder()
    {
        //Open Name and color Form

        Folder folder = new()
        {
            FolderId = Guid.NewGuid(),
            Name = $"Folder {folders.Count + 1}", //TODO enter unique name here
            IsRoot = false,
            ParentId = Guid.Parse(FolderId),
            ColorHex = "FFFFFF", //TODO set color to entered color
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        folders.Add(new(){Folder = folder, GhostComponent = true});
        
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            context.Folders.Add(folder);
            await context.SaveChangesAsync();
        }
    }
    
    
    
    private async Task CreateDeck()
    {
        //Open Name and color Form

        Deck deck = new()
        {
            DeckId = Guid.NewGuid(),
            FolderId = Guid.Parse(FolderId),
            DeckTitle = $"Decks {decks.Count + 1}", //TODO enter name 
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        decks.Add(deck);
        
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            context.Decks.Add(deck);
            await context.SaveChangesAsync();
        }
    }
}