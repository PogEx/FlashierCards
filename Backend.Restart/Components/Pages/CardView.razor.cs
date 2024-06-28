using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;
namespace Backend.Restart.Components.Pages;



public partial class CardView : ComponentBase
{
    [Parameter] public Card card { get; set; }
    [Parameter] public String CardId { get; set; }
    
    
    private bool showFront = true;
    private Card frontCard = new Card { Title = "Question", Content = "This is the front of the card." };
    private Card backCard = new Card { Title = "Answer", Content = "This is the back of the card." };

    protected override async Task OnInitializedAsync()
    {
        
    }

    private void ToggleCard()
    {
        showFront = !showFront;
    }

    public class Card
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}