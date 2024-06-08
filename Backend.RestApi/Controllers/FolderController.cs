using Backend.Common.Models.Folders;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.RestApi.Controllers;

[Route("folder")]
[Controller]
[Authorize]
public class FolderController(IFolderHandler folderHandler, IDeckHandler deckHandler) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateFolder(
        [FromBody, BindRequired] FolderCreateData data
        )
    {
        return Ok((await folderHandler.CreateFolder(
            User.GetCurrentUser(),
            data)).Value);
    }
    
    [HttpPatch]
    public async Task<IActionResult> ChangeFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id,
        [FromBody, BindRequired] FolderChangeData data
        )
    {
        await folderHandler.ChangeFolder(User.GetCurrentUser(), id, data);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id
        )
    {
        (await folderHandler.DeleteFolder(User.GetCurrentUser(), id)).Log();
        return Ok();
    }
    
    [HttpGet]
    [SwaggerResponse(200, null, typeof(FolderDto))]
    public async Task<IActionResult> GetFolder(
        [FromQuery(Name = "id"), BindRequired] Guid guid)
    {
        return Ok((await folderHandler.GetFolder(
            User.GetCurrentUser(), 
            guid)).Value);
    }
}