using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components;

public partial class FolderComponent : ComponentBase
{
    [Parameter] public Folder Folder { get; set; }
}