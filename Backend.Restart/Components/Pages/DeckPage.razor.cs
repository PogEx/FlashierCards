using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class DeckPage : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    [Inject] public NavigationManager navManager { get; set; }
    [Inject] public ILogger<DeckPage> _logger { get; set; }
    
    [Parameter] public string DeckId { get; set; }
    private Deck? deck;
    private bool _edit;
    private InputText _inputField;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                deck = await context.Decks
                    .Include(d => d.Cards.Where(c => c.BackId != null))
                    .FirstAsync(d => d.DeckId == Guid.Parse(DeckId));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
    private async Task CreateCard()
    {
        Card card = new()
        {
            CardId = Guid.NewGuid(),
            DeckId = deck.DeckId,
            Text = "",
            Type = 2,
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        
        Card frontCard = new()
        {
            CardId = Guid.NewGuid(),
            DeckId = deck.DeckId,
            BackCard = card,
            Text = "",
            Type = 2,
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        
        try
        {
            deck.Cards.Add(frontCard);
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Cards.Add(card);
                context.Cards.Add(frontCard);
                await context.SaveChangesAsync();
            }
            navManager.NavigateTo("/card/edit/" + frontCard.CardId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }

    private void CardDeleted(Card card)
    {
        deck.Cards.Remove(card);
    }

    private async Task DeleteDeck()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Decks.Remove(deck);
                await context.SaveChangesAsync();
                navManager.NavigateTo("/folder/" + deck.FolderId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
    
    private async Task SubmitDeckName(KeyboardEventArgs args)
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
                context.Decks.Update(deck);
                await context.SaveChangesAsync();
            }

            _edit = false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
}