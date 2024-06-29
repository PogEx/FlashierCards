using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;



public partial class CardView : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    private Card? FrontCard { get; set; }

    private Card? BackCard { get; set; }

    [Parameter] public string? CardId { get; set; }


    private bool _showFront = true;

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
}