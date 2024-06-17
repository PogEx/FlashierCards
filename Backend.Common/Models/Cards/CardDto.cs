namespace Backend.Common.Models.Cards;

public class CardDto
{
    public Guid CardId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; }
    
    public Guid BackId { get; set; }

    public Guid DeckId { get; set; }

    public Guid UserId { get; set; }
}