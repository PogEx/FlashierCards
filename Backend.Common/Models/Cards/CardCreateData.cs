namespace Backend.Common.Models.Cards;

public class CardCreateData
{
    public Guid DeckId { get; set; }
    public string Text { get; set; }
    
    public Guid? BackId { get; set; }
    
    public string ContentType { get; set; }
}