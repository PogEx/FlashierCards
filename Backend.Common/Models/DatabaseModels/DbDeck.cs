using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("DeckId")]
public class DbDeck
{
    public string DeckId { get; set; } = null!;

    public string DeckTitle { get; set; } = null!;
    
    public virtual ICollection<DbCardFrontBackLink> Cards { get; set; } = new List<DbCardFrontBackLink>();

    public virtual ICollection<DbFolder> Folders { get; set; } = new List<DbFolder>();

    public virtual ICollection<DbUser> Users { get; set; } = new List<DbUser>();
}
