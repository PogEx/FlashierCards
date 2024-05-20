using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models;

[PrimaryKey("DeckId")]
public class Deck
{
    public string DeckId { get; set; } = null!;

    public string DeckTitle { get; set; } = null!;
    
    public virtual ICollection<CardFrontBackLink> Cards { get; set; } = new List<CardFrontBackLink>();

    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
