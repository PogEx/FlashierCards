using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Backend.Restart.Components;

public partial class FolderComponent : ComponentBase
{
    [Parameter] public FolderGhostContainer Container { get; set; }
    

    private void SubmitFolderName(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {


            Container.GhostComponent = false;
        }
    }
}