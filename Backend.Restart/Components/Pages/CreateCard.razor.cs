using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components.Pages;

public partial class CreateCard : ComponentBase
{
    private bool withback = false;
    public void Button_click()
    {
        withback = true;
    }
}