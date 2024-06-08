using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("DeckId")]
public class Deck
{
    public Guid DeckId { get; set; }

    public string DeckTitle { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public DeckInviteCode? InviteCode = null!;
}
