﻿namespace Backend.Common.Models.Decks;

public class DeckDto
{
    
    public DeckDto(){}

    public Guid DeckId { get; set; } = default;
    public string Name { get; set; } = null!;

    public ICollection<Guid> Cards { get; set; } = new List<Guid>();
}