using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey(propertyName: "FrontCardId")]
public class CardFrontBackLink
{
    
    public Guid FrontCardId { get; set; }

    public Guid BackCardId { get; set; }

    public virtual Card BackCard { get; set; } = null!;

    public virtual Card FrontCard { get; set; } = null!;

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
}
