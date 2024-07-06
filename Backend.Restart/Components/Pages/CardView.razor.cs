using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class CardView : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ILogger<CardView> _logger { get; set; }
    private Card? FrontCard { get; set; }

    private Card? BackCard { get; set; }

    [Parameter] public required string CardId { get; set; }
    
    private bool _showFront = true;
    
    private List<Card> _cardList = new ();
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                FrontCard = await context.Cards
                    .Include(c => c.BackCard)
                    .Include(c => c.Deck)
                    .FirstAsync(c => c.CardId == Guid.Parse(CardId));

                BackCard = FrontCard?.BackCard;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }

    private void ToggleCard()
    {
        _showFront = !_showFront;
    }

    private async Task SaveCard()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Update(FrontCard);
                context.Update(BackCard);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
    
    private async Task PreviousCard()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                _cardList = await context.Cards.Where(c =>
                    c.BackId != null && FrontCard != null && c.DeckId == FrontCard.DeckId &&
                    c.CardId != FrontCard.CardId).ToListAsync();
            }

            if (_cardList.Count != 0)
            {
                Navigation.NavigateTo($"/Card/View/{_cardList.Random().CardId}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }

    private async Task NextCard()
    { 
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                _cardList = await context.Cards.Where(c =>
                    c.BackId != null && FrontCard != null && c.DeckId == FrontCard.DeckId &&
                    c.CardId != FrontCard.CardId).ToListAsync();
            }

            if (_cardList.Count != 0)
            {
                Navigation.NavigateTo($"/Card/View/{_cardList.Random().CardId}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }

    private async Task DeleteCard()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Cards.Remove(FrontCard);
                await context.SaveChangesAsync();
                Navigation.NavigateTo("/deck/" + FrontCard.DeckId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
}