using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components.Pages;

public partial class DeckPage : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    
    [Parameter] public string DeckId { get; set; }
    private List<Card> Cards = new();
    
    protected override async Task OnInitializedAsync()
    {
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            Deck deck = await context.Decks
                .Include(d=> d.Cards)
                .FirstAsync(d => d.DeckId == Guid.Parse(DeckId));
            Cards = deck.Cards.Where(c => c.BackId != null).ToList();
        }
    }
    private async Task CreateCard()
    {
        Card card = new()
        {
            CardId = Guid.NewGuid(),
            DeckId = Guid.Parse(DeckId),
            Text = "",
            Type = 2,
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        
        Card frontCard = new()
        {
            CardId = Guid.NewGuid(),
            DeckId = Guid.Parse(DeckId),
            BackCard = card,
            Text = "",
            Type = 2,
            UserId = Guid.Parse("e87f8052-cf90-43e4-900d-b75239d4b08f")
        };
        
        Cards.Add(frontCard);
        
        using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
        {
            context.Cards.Add(card);
            context.Cards.Add(frontCard);
            await context.SaveChangesAsync();
        }
    }
}