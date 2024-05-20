﻿using System.Security.Claims;
using Backend.Common.Models.Folders;
using Backend.RestApi.Contracts.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.RestApi.Controllers;

[Route("folder")]
[Controller]
[Authorize]
public class FolderController(IFolderHandler folderHandler) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateFolder(
        [FromBody, BindRequired] FolderCreateData data
        )
    {
        return Ok(await folderHandler.CreateFolder(new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
            data.Parent, data.Name));
    }
    
    [HttpPatch]
    public async Task<IActionResult> ChangeFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id,
        [FromBody, BindRequired] FolderChangeData data
        )
    {
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFolder(
        [FromQuery(Name = "id"), BindRequired] Guid id
        )
    {
        await folderHandler.DeleteFolder(id);
        return Ok();
    }
    
    [HttpGet("userroot")]
    public async Task<IActionResult> GetUserRoot()
    {
        return Ok(await folderHandler.GetUserRoot(
            new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)));
    }

    [HttpGet("parent")]
    public async Task<IActionResult> GetParent(
        [FromQuery(Name = "id"), BindRequired] Guid guid
        )
    {
        Guid? parentId = await folderHandler.GetParentFolder(guid);
        return Ok(parentId ?? guid);
    }

    [HttpGet]
    public async Task<IActionResult> GetChildren(
        [FromQuery(Name = "Id"), BindRequired] Guid guid)
    {
        return Ok(await folderHandler.GetChildren(guid));
    }
}