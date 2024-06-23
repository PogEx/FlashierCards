using Backend.Common.Models.Decks;

namespace Backend.Common.Models.Folders;

public class FolderDto
{
    public Guid FolderId { get; set; }

    public Guid UserId { get; set; }

    public bool IsRoot { get; set; }

    public required string Name { get; set; }
    
    public required string Color { get; set; }

    public Guid? ParentId { get; set; }

    public IEnumerable<FolderDto> ChildrenIds { get; set; } = new List<FolderDto>();
    public IEnumerable<DeckDto> DeckIds { get; set; } = new List<DeckDto>();

    public bool IsEmpty { get; set; } = true;
}