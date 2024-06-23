namespace Backend.Common.Models.Decks;

public class DeckDto
{
    public Guid DeckId { get; set; }
    public required string Name { get; set; }
}