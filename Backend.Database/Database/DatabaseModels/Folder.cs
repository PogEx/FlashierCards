using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("FolderId")]
public class Folder
{
    public Guid FolderId { get; set; }

    public Guid UserId { get; set; }

    public bool IsRoot { get; set; }

    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public string ColorHex { get; set; } = null!;

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();

    public virtual ICollection<Folder> Children { get; set; } = new List<Folder>();

    public virtual Folder? Parent { get; set; }

    public virtual User User { get; set; } = null!;
}
