using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components;

public partial class FolderComponent : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    [Parameter] public GhostContainer<Folder> Container { get; set; }

    private InputText _inputField;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if( Container.GhostComponent && _inputField?.Element != null) 
                await _inputField.Element.Value.FocusAsync();
        }
    }
    
    private async Task SubmitFolderName(KeyboardEventArgs args)
    {
        if (Container.GhostComponent && args.Key == "Enter")
        {
            await SaveToDatabase();
        }
    }
    
    public async Task lostFocus(FocusEventArgs args)
    {
        if(Container.GhostComponent)
            await SaveToDatabase();
    }

    private async Task SaveToDatabase()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Folders.Add(Container.Payload);
                await context.SaveChangesAsync();
            }

            Container.GhostComponent = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}