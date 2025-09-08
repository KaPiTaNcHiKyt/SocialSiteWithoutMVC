using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.Extensions;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatsController(ChatService service, IHttpContextAccessor context) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> SendMessage([Required] string text, [Required] string loginTo)
    {
        if (context.HttpContext is null)
            return BadRequest();
        
        var loginFrom = context.HttpContext.Request.Cookies["userLogin"];
        
        if (loginFrom is null)
            return BadRequest("Cookie not found, authorize again");
            
        var result = await service.AddMessage(text, loginFrom, loginTo);
        
        return result ? Ok() : BadRequest();
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<ChatModel>> GetChat([Required] string loginTo)
    {
        if (context.HttpContext is null)
            return BadRequest();
        
        var loginFrom = context.HttpContext.Request.Cookies["userLogin"];
        
        if (loginFrom is null)
            return BadRequest("Cookie not found, authorize again");
        
        var chat = await service.GetChat(loginFrom, loginTo);
        
        if (chat is not null)
            return Ok(chat.ToModel());
        
        return NotFound();
    }
}