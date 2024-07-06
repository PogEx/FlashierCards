using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components;

public partial class CardComponent : ComponentBase
{
    
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    [Inject] public ILogger<CardComponent> _logger { get; set; }
    [Parameter] public Card Card { get; set; }
    [Parameter] public EventCallback<Card> OnCardDeleted { get; set; }

    private async Task DeleteCard()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Cards.Remove(Card);
                await context.SaveChangesAsync();
                await OnCardDeleted.InvokeAsync(Card);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }
}
