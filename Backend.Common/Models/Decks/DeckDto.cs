namespace Backend.Common.Models.Decks;

public struct DeckDto
{
    
    public DeckDto(){}

    public Guid DeckId { get; set; } = default;
    public string Name { get; set; } = null!;
}