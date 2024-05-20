namespace Backend.Common.Models;

public partial class FolderParentChildLink
{
    public Guid? ParentFolderId { get; set; }

    public Guid? ChildFolderId { get; set; }

    public virtual Folder ChildFolder { get; set; } = null!;

    public virtual Folder ParentFolder { get; set; } = null!;
}
