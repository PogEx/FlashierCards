using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey(propertyName: "FrontCardId")]
public class DbCardFrontBackLink
{
    
    public Guid FrontCardId { get; set; }

    public Guid BackCardId { get; set; }

    public virtual DbCard BackDbCard { get; set; } = null!;

    public virtual DbCard FrontDbCard { get; set; } = null!;

    public virtual ICollection<DbDeck> Decks { get; set; } = new List<DbDeck>();
}
