namespace Backend.Common.Models.Folders;

public class Folder
{
    public Guid Parent { get; set; }
    public string? Name { get; set; }
    public string? ColorHex { get; set; }
    public Guid Owner { get; set; }
}