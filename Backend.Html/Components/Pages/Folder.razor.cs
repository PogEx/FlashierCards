using Backend.Common.Models.Decks;
using Backend.Common.Models.Folders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using RestSharp;

namespace Backend.Html.Components.Pages;

public partial class Folder : ComponentBase
{
    private List<FolderDto> children = new();
    private List<DeckDto> decks = new();
    
    protected override async Task OnInitializedAsync()
    {
        Uri uri = navManager.ToAbsoluteUri(navManager.Uri);

        RestRequest request = new("/folder");
        request.AddHeader("Authorization", "Bearer " + await authHandler.GetToken());
        
        IRestClient client = await clientProvider.GetRestClient();
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var folderId))
        {
            request.AddQueryParameter("id", folderId);
        }
        FolderDto? folder = await client.GetAsync<FolderDto>(request);
        children = folder.ChildrenIds.ToList();
        decks = folder.DeckIds.ToList();
    }
}