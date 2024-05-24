using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("FolderId")]
public class DbFolder
{
    public Guid FolderId { get; set; }

    public Guid UserId { get; set; }

    public bool IsRoot { get; set; }

    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public string ColorHex { get; set; } = null!;

    public virtual ICollection<DbDeck> Decks { get; set; } = new List<DbDeck>();

    public virtual ICollection<DbFolder> Children { get; set; } = new List<DbFolder>();

    public virtual DbFolder? Parent { get; set; }

    public virtual DbUser DbUser { get; set; } = null!;
}
