namespace Backend.Common.Models;

public partial class Folder
{
    public Guid? FolderId { get; set; }

    public Guid? UserId { get; set; }

    public bool IsRoot { get; set; }

    public string Name { get; set; } = null!;

    public virtual FolderDeckLink? FolderDeckLink { get; set; }

    public virtual ICollection<FolderParentChildLink> FolderParentChildLinkChildFolders { get; set; } = new List<FolderParentChildLink>();

    public virtual FolderParentChildLink? FolderParentChildLinkParentFolder { get; set; }

    public virtual User User { get; set; } = null!;
}
