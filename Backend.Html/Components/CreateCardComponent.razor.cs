using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Backend.Html.Components;

public partial class CreateCardComponent : ComponentBase
{
    [Parameter] public string Title { get; set; }
    
}