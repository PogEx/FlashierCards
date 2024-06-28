using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components;

public partial class CardComponent : ComponentBase
{
    [Parameter] public Card Card { get; set; }
}
