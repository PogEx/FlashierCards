using Backend.Database.Database.DatabaseModels;

namespace Backend.Restart.Models;

public class FolderGhostContainer
{
    public Folder Folder { get; set; }
    
    public bool GhostComponent { get; set; } = false;
}