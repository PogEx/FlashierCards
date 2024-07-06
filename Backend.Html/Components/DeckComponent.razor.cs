using Backend.Common.Models.Decks;
using Microsoft.AspNetCore.Components;

namespace Backend.Html.Components;

public partial class DeckComponent : ComponentBase
{
    [Parameter] public DeckDto Deck { get; set; }
}