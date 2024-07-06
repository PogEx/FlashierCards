using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components;

public partial class CardViewComponent : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
}