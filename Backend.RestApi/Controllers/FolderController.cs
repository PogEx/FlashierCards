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
    [HttpGet]
    public IActionResult GetFolder(
        [FromQuery(Name = "id")] Guid? id
        )
    {
        if (id is null) return GetUserRoot();
        
        
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateFolder(
        [FromBody, BindRequired] FolderCreateData data
        )
    {
        return Ok();
    }
    
    [HttpPatch]
    public IActionResult ChangeFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id,
        [FromBody, BindRequired] FolderChangeData data
        )
    {
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult DeleteFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id
        )
    {
        return Ok();
    }
    
    [HttpGet("userroot")]
    public IActionResult GetUserRoot()
    {
        return Ok();
    }

    [HttpGet("parent")]
    public IActionResult GetParent(
        [FromQuery(Name = "id"), BindRequired] Guid guid
        )
    {
        return Ok();
    }
}