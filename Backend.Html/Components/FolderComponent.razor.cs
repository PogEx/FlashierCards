using Backend.Common.Models.Folders;
using Microsoft.AspNetCore.Components;

namespace Backend.Html.Components;

public partial class FolderComponent : ComponentBase
{
    [Parameter] public FolderDto Folder { get; set; }
}