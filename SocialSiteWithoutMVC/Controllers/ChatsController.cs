using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.Extensions;
using SocialSiteWithoutMVC.Interfaces;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatsController(ChatService chatService, JwtService jwtService, IHttpContextAccessor context) 
    : ControllerBase, ITestings
{
    [HttpPost("[action]")]
    public async Task<IActionResult> SendMessage([Required] string text, [Required] string loginTo)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        
        var resultAdd = await chatService.AddMessage(text, resultTest.resultCookie!, loginTo);
        
        return resultAdd ? Ok() : BadRequest();
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<ChatModel>> GetChat([Required] string loginTo)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        
        var chat = await chatService.GetChat(resultTest.resultCookie!, loginTo);
        
        if (chat is not null)
            return Ok(chat.ToModel());
        
        return NotFound();
    }

    public bool MainTests()
        => context.HttpContext is not null;

    public (bool isConfirmTest, string? resultCookie) MainTests(string cookieName)
    {
        if (context.HttpContext is null)
            return (false, null);

        var jwt = context.HttpContext.Request.Cookies[cookieName];
        if (jwt is null || jwtService.GetLogin(jwt) is not { } login)
            return (false, null);
        
        return (true, login);
    }
}