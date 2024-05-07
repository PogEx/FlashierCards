using Backend.Common.Models.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("folder")]
[Controller]
[Authorize]
public class FolderController : Controller
{
    [HttpGet("userroot")]
    public IActionResult GetUserRoot()
    {
        return Ok();
    }

    [HttpGet("parent")]
    public IActionResult GetParent([FromQuery, BindRequired] Guid guid)
    {
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateFolder(
        [FromBody, BindRequired] FolderCreateData createData)
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult GetFolder([FromQuery] Guid folderId)
    {
        return Ok();
    }
    
    [HttpPatch]
    public IActionResult ChangeFolder(
        [FromQuery] Guid folderId,
        [FromBody] FolderChangeData changeData)
    {
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult DeleteFolder([FromQuery] Guid folderId)
    {
        return Ok();
    }
}