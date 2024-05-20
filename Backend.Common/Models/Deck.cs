namespace Backend.Common.Models;

public partial class Deck
{
    public string DeckId { get; set; } = null!;

    public string DeckTitle { get; set; } = null!;

    public virtual ICollection<FolderDeckLink> FolderDeckLinks { get; set; } = new List<FolderDeckLink>();

    public virtual ICollection<CardFrontBackLink> Cards { get; set; } = new List<CardFrontBackLink>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
