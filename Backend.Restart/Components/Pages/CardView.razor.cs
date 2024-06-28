using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Backend.Restart.Components.Pages;



public partial class CardView : ComponentBase
{
    [Parameter] public Card frontCard { get; set; }

    [Parameter] public Card backCard { get; set; }

    [Parameter] public String CardId { get; set; }

    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }

    private bool showFront = true;

    protected override async Task OnInitializedAsync()
    {
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            Guid cardGuid = Guid.Parse(CardId);
            frontCard = await context.Cards
                .Include(c => c.BackCard)
                .FirstAsync(c => c.CardId == cardGuid);

            backCard = frontCard.BackCard;
        }
    }

    private void ToggleCard()
    {
        showFront = !showFront;
    }
}