using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("CardId")]
public class Card
{
    public Guid CardId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; } = null!;
    
    public Guid? BackId { get; set; }

    public Guid DeckId { get; set; }

    public Guid UserId { get; set; }

    public virtual Card? BackCard { get; set; }

    public virtual Deck Deck { get; set; } = null!;

    public virtual Card? FrontCard { get; set; }

    public virtual CardType TypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
