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
public class MeController(UserService userService, EditUserService editUserService, ChatService chatService, IHttpContextAccessor context) 
    : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<(UserModel, ChatModel)>> GetMe()
    {
        var resultTest = MainTests(["userlogin"]);
        if (!resultTest.isConfirmTest)
            return BadRequest();

        var me = await userService.GetMe(resultTest.cookies![0]);
        var myChats = await chatService.GetAllByLogin(resultTest.cookies[0]);

        var meModel = me.ToModel();
        meModel.ChatModels = myChats?.Select(c => c.ToModelWithoutMessages()).ToArray();

        return Ok(meModel);
    }
    
    [HttpPatch("[action]")]
    public async Task<IActionResult> EditPassword([Required] string newPassword)
    {
        var resultTest = MainTests(["userlogin"]);
        if (!resultTest.isConfirmTest)
            return BadRequest();
        
        await editUserService.PatchPassword(resultTest.cookies![0], newPassword);
        
        return Ok();
    }
    
    [HttpPatch("[action]")]
    public async Task<IActionResult> EditNickname([Required] string newNickName)
    {
        var resultTest = MainTests(["userlogin"]);
        if (!resultTest.isConfirmTest)
            return BadRequest();

        await editUserService.PatchNickname(resultTest.cookies![0], newNickName);
        
        return Ok();
    }
    
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAccount(string login, string password)
    {
        if (!MainTests())
            return BadRequest();
        
        context.HttpContext!.Response.Cookies.Delete("userLogin");
        context.HttpContext.Response.Cookies.Delete("tasty-cookies");
        
        await userService.Delete(login, password);
        return Ok();
    }

    private bool MainTests()
        => context.HttpContext is not null;

    private (bool isConfirmTest, string[]? cookies) MainTests(string[] cookiesNames)
    {
        if (context.HttpContext is null)
            return (false, null);

        List<string> resultCookiesNames = new(4);
        string? tempCookieName;

        foreach (var cookieName in cookiesNames)
        {
            tempCookieName = context.HttpContext.Request.Cookies[cookieName];
            if (tempCookieName is null)
                return (false, null);
            resultCookiesNames.Add(tempCookieName);
        }

        return (true, resultCookiesNames.ToArray());
    }
}