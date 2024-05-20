namespace Backend.Common.Models;

public partial class FolderDeckLink
{
    public Guid? FolderId { get; set; }
    public Guid? DeckId { get; set; }
    public virtual Deck Deck { get; set; } = null!;

    public virtual Folder Folder { get; set; } = null!;
}
