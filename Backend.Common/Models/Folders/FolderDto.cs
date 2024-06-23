namespace Backend.Common.Models.Folders;

public struct FolderDto
{
    public FolderDto()
    {
    }

    public Guid FolderId { get; set; } = default;

    public Guid UserId { get; set; } = default;

    public bool IsRoot { get; set; } = false;

    public string Name { get; set; } = null!;
    
    public string Color { get; set; }

    public Guid? ParentId { get; set; } = null!;

    public IEnumerable<Guid> ChildrenIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> DeckIds { get; set; } = new List<Guid>();

    public bool IsEmpty { get; set; } = true;
}