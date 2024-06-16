using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("CardId")]
public class Card
{
    public Guid CardId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; } = null!;
    
    public Guid BackId { get; set; }

    public virtual Card Back { get; set; } = null!;
    public virtual Card Front { get; set; } = null!;
    
    public virtual ICollection<Deck> Deck { get; set; } = null!;
    public virtual CardType TypeNavigation { get; set; } = null!;
}
