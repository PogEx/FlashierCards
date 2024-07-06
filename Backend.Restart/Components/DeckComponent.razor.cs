using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.Restart.Extensions;
using Backend.Restart.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Backend.Restart.Components;

public partial class DeckComponent : ComponentBase
{
    [Inject] public IDbContextFactory<FlashiercardsContext> DbContextFactory { get; set; }
    [Inject] public ILogger<DeckComponent> _logger { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Parameter] public GhostContainer<Deck> Container { get; set; }

    private InputText _inputField;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Container.GhostComponent && _inputField?.Element != null)
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
        if (Container.GhostComponent)
            await SaveToDatabase();
    }

    private async Task SaveToDatabase()
    {
        try
        {
            using (FlashiercardsContext context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Decks.Add(Container.Payload);
                await context.SaveChangesAsync();
            }

            Container.GhostComponent = false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A Database Error occured");
        }
    }

    private Task OpenRandomCard()
    {
        Card? card = Container.Payload.Cards.Where(c => c.BackId != null).Random();
        if (card is not null)
        {
            NavManager.NavigateTo("/Card/View/" + card.CardId);
            return Task.CompletedTask;
        }

        NavManager.NavigateTo("/Deck/" + Container.Payload.DeckId);
        return Task.CompletedTask;
    }
}