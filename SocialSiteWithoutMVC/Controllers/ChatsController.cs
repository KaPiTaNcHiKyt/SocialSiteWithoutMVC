using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.infrastructureLogic.Services;
using SocialSiteWithoutMVC.Interfaces;
using SocialSiteWithoutMVC.Mapper;
using SocialSiteWithoutMVC.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/Chats")]
[Authorize]
[EnableRateLimiting("Default")]
public class ChatsController(ChatService chatService, JwtService jwtService, IHttpContextAccessor context)
    : ControllerBase, ITestings
{
    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage([Required] string text, [Required] string loginTo)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        var resultAdd = await chatService.AddMessage(text, resultTest.resultCookie!, loginTo);
        return resultAdd ? Ok() : BadRequest();
    }
    
    [HttpGet("GetChatWithOneUser")]
    [DisableRateLimiting]
    public async Task<ActionResult<ChatModel>> GetChat([Required] string loginTo, [FromServices] IMemoryCache cache)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        if (cache.TryGetValue($"{resultTest.resultCookie!}_{loginTo}", out ChatModel? chatModel))
            return Ok(chatModel);
        var chat = await chatService.GetChat(resultTest.resultCookie!, loginTo);
        if (chat == null)
            return NotFound();
        chatModel = ModelMapper.ChatEntityToModel(chat);
        chatModel.UsersLogin = [loginTo];
        cache.Set($"{resultTest.resultCookie!}_{loginTo}", chatModel, TimeSpan.FromSeconds(1));
        return Ok(chatModel);
    }

    [HttpPost("SendMessageToGroup")]
    public async Task<IActionResult> SendMessageToGroup([Required] string text, [Required] string groupName)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        var resultAdd = await chatService.AddMessageToGroup(text, resultTest.resultCookie!, groupName);
        return resultAdd ? Ok() : BadRequest();
    }

    [HttpPost("CreateGroup")]
    public async Task<IActionResult> CreateGroup([Required] string groupName, [Required] params string[] logins)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        var resultAdd = await chatService.CreateGroup(resultTest.resultCookie!, logins, groupName);
        return resultAdd ? Ok() : BadRequest("Group already exists or users not found");
    }
    
    [HttpGet("GetGroupChat")]
    public async Task<ActionResult<ChatModel>> GetGroupChat([Required] string groupName, [FromServices] IMemoryCache cache)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest("Cookie not found, authorize again");
        if (cache.TryGetValue($"{resultTest.resultCookie!}_{groupName}", out ChatModel? chatModel))
            return Ok(chatModel);
        var chat = await chatService.GetGroupChat(resultTest.resultCookie!, groupName);
        if (chat == null)
            return NotFound();
        chatModel = ModelMapper.ChatEntityToModel(chat);
        // chatModel.UsersLogin = [loginTo];
        cache.Set($"{resultTest.resultCookie!}_{groupName}", chatModel, TimeSpan.FromSeconds(1));
        return Ok(chatModel);
    }

    [SwaggerIgnore]
    public bool MainTests()
        => context.HttpContext is not null;

    [SwaggerIgnore]
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