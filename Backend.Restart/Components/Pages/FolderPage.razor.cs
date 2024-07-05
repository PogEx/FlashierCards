using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Extensions;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class FolderPage : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    [Inject] public NavigationManager navManager { get; set; }
    
    [Parameter] public string? FolderId { get; set; }
    private Folder? folder;
    private List<GhostContainer<Folder>> _folders = new();
    private List<GhostContainer<Deck>> _decks = new();
    private bool _edit;
    private InputText _inputField;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
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

                _folders = folder.Children
                    .OrderBy(f => f.Name.Length)
                    .ThenBy(f => f.Name)
                    .MapTo(f => new GhostContainer<Folder> { Payload = f })
                    .ToList();
                _decks = folder.Decks
                    .OrderBy(d => d.DeckTitle.Length)
                    .ThenBy(d => d.DeckTitle)
                    .MapTo(d => new GhostContainer<Deck> { Payload = d })
                    .ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task CreateFolder()
    {
        //Open Name and color Form

        try
        {
            Folder folder = new()
            {
                FolderId = Guid.NewGuid(),
                Name = $"Folder {_folders.Count + 1}",
                IsRoot = false,
                ParentId = Guid.Parse(FolderId),
                ColorHex = "FFFFFF", 
                UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
            };
            _folders.Add(new() { Payload = folder, GhostComponent = true });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    
    
    private async Task CreateDeck()
    {
        //Open Name and color Form

        try
        {
            Deck deck = new()
            {
                DeckId = Guid.NewGuid(),
                FolderId = Guid.Parse(FolderId),
                DeckTitle = $"Deck {_decks.Count + 1}",
                UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
            };
            _decks.Add(new() { Payload = deck, GhostComponent = true });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task DeleteFolder()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Folders.Remove(folder);
                await context.SaveChangesAsync();
                navManager.NavigateTo("/folder/" + folder.ParentId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    private async Task SubmitFolderName(KeyboardEventArgs args)
    {
        if (_edit && args.Key == "Enter")
        {
            await SaveToDatabase();
        }
    }
    
    public async Task lostFocus(FocusEventArgs args)
    {
        if(_edit)
            await SaveToDatabase();
    }
    
    private async Task SaveToDatabase()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Folders.Update(folder);
                await context.SaveChangesAsync();
            }

            _edit = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}