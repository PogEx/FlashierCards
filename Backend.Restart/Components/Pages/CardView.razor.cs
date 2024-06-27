using Backend.Database.Database.DatabaseModels;
using Microsoft.AspNetCore.Components;

namespace Backend.Restart.Components.Pages;

public partial class CardView : ComponentBase
{
    private bool showFront = true;
    private Card frontCard = new Card();
    private Card backCard = new Card();
    private async void ShowCard()
    {
        frontCard = new Card()
         {
            // Deck = Card
             //user deck cardid
             
             
         };
        
        backCard = new Card()
        {

        };
        
        
    }
    
    public void ShowBackSide()
    {
        showFront = false;
    }

    public void ShowFrontSide()
    {
        showFront = true;
    }

}