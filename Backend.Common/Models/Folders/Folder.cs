using Backend.Common.Models.DatabaseModels;

namespace Backend.Common.Models.Folders;

public struct Folder
{
    public Folder()
    {
    }
    
    public Folder(DbFolder dbFolder)
    {
        FolderId = dbFolder.FolderId;
        UserId = dbFolder.UserId;
        IsRoot = dbFolder.IsRoot;
        Name = dbFolder.Name;
        ParentId = dbFolder.ParentId;
        IsEmpty = dbFolder.Children.Count == 0;
    }

    public Guid FolderId { get; set; } = default;

    public Guid UserId { get; set; } = default;

    public bool IsRoot { get; set; } = false;

    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; } = null!;

    public bool IsEmpty { get; set; } = true;
}