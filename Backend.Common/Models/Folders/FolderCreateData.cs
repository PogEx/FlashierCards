namespace Backend.Common.Models.Folders;

public class FolderCreateData
{
    public Guid Parent { get; set; }
    public string Name { get; set; }
}