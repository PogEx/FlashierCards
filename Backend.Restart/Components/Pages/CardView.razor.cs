using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class CardView : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Inject] private NavigationManager Navigation { get; set; }
    private Card? FrontCard { get; set; }

    public Boolean edit = false;

    private Card? BackCard { get; set; }

    [Parameter] public string? CardId { get; set; }


    private bool _showFront = true;
    private List<Card> _cardList = new List<Card>();


    protected override async Task OnInitializedAsync()
    {
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            FrontCard = await context.Cards
                .Include(c => c.BackCard)
                .FirstAsync(c => c.CardId == Guid.Parse(CardId));
            
            BackCard = FrontCard?.BackCard;
        }
    }

    private void ToggleCard()
    {
        _showFront = !_showFront;
    }

    private void EditCard()
    {
            edit = !edit;
    }
    
    private void PreviousCard()
    {
        Navigation.NavigateTo("/Card/View/");
    }

    private async Task NextCard()
    { 
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            _cardList = await context.Cards.Where(c => c.DeckId == c.DeckId).ToListAsync();
        }
        
        Navigation.NavigateTo($"/Card/View/{_cardList[1]}");
    }
    
}