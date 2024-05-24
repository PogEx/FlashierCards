using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("CardId")]
public class DbCard
{
    public Guid CardId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; } = null!;

    public virtual ICollection<DbCardFrontBackLink> CardFrontBackLinkBackCards { get; set; } = new List<DbCardFrontBackLink>();

    public virtual DbCardFrontBackLink? CardFrontBackLinkFrontCard { get; set; }

    public virtual DbCardType TypeNavigation { get; set; } = null!;
}
