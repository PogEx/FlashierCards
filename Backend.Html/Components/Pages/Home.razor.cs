using Microsoft.AspNetCore.Components;

namespace Backend.Html.Components.Pages;

public partial class Home : ComponentBase
{
    
    private void GotoLogin()
    {
        navManager.NavigateTo("/");
    }
    
}