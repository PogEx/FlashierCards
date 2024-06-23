using Backend.Common.Extensions;
using Backend.Database.Database.DatabaseModels;

namespace Backend.RestApi.Helpers.Extensions;

public static class CloneExtensions
{
    public static Deck Clone(this Deck deck)
    {
        return new Deck
        {
            DeckId = Guid.NewGuid(),
            DeckTitle = deck.DeckTitle,
            FolderId = deck.FolderId,
            UserId = deck.UserId,
            Cards = (ICollection<Card>) deck.Cards.MapTo(c => c.Clone())
        };
    }

    public static Card Clone(this Card card)
    {
        return new Card
        {
            CardId = Guid.NewGuid(),
            Type = card.Type,
            Text = card.Text,
            BackId = card.BackId,
            DeckId = card.DeckId,
            UserId = card.UserId
        };
    }
}