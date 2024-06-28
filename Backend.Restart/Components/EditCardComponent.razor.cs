using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components;

public partial class EditCardComponent : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string Content { get; set; }
}