using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.Extensions;
using SocialSiteWithoutMVC.Interfaces;
using SocialSiteWithoutMVC.Mapper;
using SocialSiteWithoutMVC.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/MeController")]
[Authorize]
public class MeController(UserService userService, EditUserService editUserService, JwtService jwtService, IHttpContextAccessor context) 
    : ControllerBase, ITestings
{
    [HttpGet("GetMe")]
    public async Task<ActionResult<UserModel>> GetMe([FromServices] ChatService chatService)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest();
        var me = await userService.GetUser(resultTest.resultCookie!);
        if (me is null)
            return NotFound();
        var myChats = await chatService.GetAllByLogin(resultTest.resultCookie!);
        var meModel = ModelMapper.UserEntityToModel(me);
        meModel.Chats = myChats?
            .Select(c => 
                ModelMapper.ChatEntityToModelWithoutMessages(c, resultTest.resultCookie!))
            .ToList();
        return Ok(meModel);
    }
    
    [HttpPatch("EditPassword")]
    public async Task<IActionResult> EditPassword([Required] string newPassword)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest();
        await editUserService.PatchPassword(resultTest.resultCookie!, newPassword);
        return Ok();
    }
    
    [HttpPatch("EditNickname")]
    public async Task<IActionResult> EditNickname([Required] string newNickName)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest();
        await editUserService.PatchNickname(resultTest.resultCookie!, newNickName);
        return Ok();
    }
    
    [HttpDelete("DeleteThisAccount")]
    public async Task<IActionResult> DeleteAccount([Required] string login, [Required] string password)
    {
        if (!MainTests())
            return BadRequest();
        context.HttpContext!.Response.Cookies.Delete("tasty-cookies");
        await userService.Delete(login, password);
        return Ok();
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