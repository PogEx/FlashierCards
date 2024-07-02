using Microsoft.AspNetCore.Components;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class EditCard : ComponentBase
{
     [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Inject] private NavigationManager Navigation { get; set; }
    private Card? FrontCard { get; set; }

    public Boolean edit = false;

    private Card? BackCard { get; set; }

    [Parameter] public required string CardId { get; set; }
    
    private bool _showFront = true;
    
    private List<Card> _cardList = new ();
    
    protected override async Task OnInitializedAsync()
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

    private Task EditCardBtn()
    {
        edit = !edit;
        return Task.CompletedTask;
    }

    private async Task SaveCard()
    {
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            context.Update(FrontCard);
            context.Update(BackCard);
            await context.SaveChangesAsync();
        }
        Navigation.NavigateTo("/Card/View/" + FrontCard.CardId);
    }
}