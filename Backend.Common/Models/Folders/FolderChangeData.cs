namespace Backend.Common.Models.Folders;

public class FolderChangeData
{
    public Guid? Parent { get; set; }
    public string? Name { get; set; }
    public string? ColorHex { get; set; }
}