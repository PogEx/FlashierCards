using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("DeckId")]
public class Deck
{
    public Guid DeckId { get; set; }

    public string DeckTitle { get; set; } = null!;

    public Guid FolderId { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual DeckInviteCode? DeckInviteCode { get; set; }

    public virtual Folder Folder { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
