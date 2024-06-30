using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Extensions;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class FolderPage : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Parameter] public string? FolderId { get; set; }
    private string _name = "";
    private List<GhostContainer<Folder>> _folders = new();
    private List<GhostContainer<Deck>> _decks = new();
    private Guid? parent;

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
                    .ThenInclude(d => d.Cards)
                    .FirstAsync(folder => folder.IsRoot == true);
            }
            else
            {
                Guid folderGuid = Guid.Parse(FolderId);
                folder = await context.Folders
                    .Include(f => f.Children)
                    .Include(f => f.Decks)
                    .ThenInclude(d => d.Cards)
                    .FirstOrDefaultAsync(folder => folder.FolderId == folderGuid);
            }
            
            FolderId = folder.FolderId.ToString();
            _name = folder.Name;
            parent = folder.ParentId;
            _folders = folder.Children
                .OrderBy(f => f.Name.Length)
                .ThenBy(f => f.Name)
                .MapTo(f => new GhostContainer<Folder>{Payload = f})
                .ToList();
            _decks = folder.Decks
                .OrderBy(d => d.DeckTitle.Length)
                .ThenBy(d => d.DeckTitle)
                .MapTo(d => new GhostContainer<Deck>{Payload = d})
                .ToList();
        }
    }
    
    private async Task CreateFolder()
    {
        //Open Name and color Form

        Folder folder = new()
        {
            FolderId = Guid.NewGuid(),
            Name = $"Folder {_folders.Count + 1}", //TODO enter unique name here
            IsRoot = false,
            ParentId = Guid.Parse(FolderId),
            ColorHex = "FFFFFF", //TODO set color to entered color
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        _folders.Add(new(){Payload = folder, GhostComponent = true});
    }
    
    
    
    private async Task CreateDeck()
    {
        //Open Name and color Form

        Deck deck = new()
        {
            DeckId = Guid.NewGuid(),
            FolderId = Guid.Parse(FolderId),
            DeckTitle = $"Deck {_decks.Count + 1}", //TODO enter name 
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        _decks.Add(new(){Payload = deck, GhostComponent = true});
    }
}