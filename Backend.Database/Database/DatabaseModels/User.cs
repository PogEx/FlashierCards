using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("UserId")]
public class User
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();

    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();

    public virtual UserSetting? UserSetting { get; set; }
}
