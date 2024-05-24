using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("UserId")]
public class DbUser
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<DbFolder> Folders { get; set; } = new List<DbFolder>();

    public virtual DbUserSetting? UserSetting { get; set; }

    public virtual ICollection<DbDeck> Decks { get; set; } = new List<DbDeck>();
}
