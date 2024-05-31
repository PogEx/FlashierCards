using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("CardId")]
public class Card
{
    public Guid CardId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; } = null!;

    public virtual ICollection<CardFrontBackLink> CardFrontBackLinkBackCards { get; set; } = new List<CardFrontBackLink>();

    public virtual CardFrontBackLink? CardFrontBackLinkFrontCard { get; set; }

    public virtual CardType TypeNavigation { get; set; } = null!;
}
