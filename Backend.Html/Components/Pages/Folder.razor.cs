using Backend.Common.Models.Decks;
using Backend.Common.Models.Folders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Backend.Html.Components.Pages;

public partial class Folder : ComponentBase
{
    private List<FolderDto> children = new();
    private List<DeckDto> decks = new();
    
    protected override void OnInitialized()
    {
        Uri uri = navManager.ToAbsoluteUri(navManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var folderId))
        {
            //Fetch known folder
        }
        //Fetch User Root instead
    }
}