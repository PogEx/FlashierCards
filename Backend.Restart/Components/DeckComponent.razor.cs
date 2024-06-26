using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components;

public partial class DeckComponent : ComponentBase
{
    [Parameter] public Deck Deck { get; set; }
}