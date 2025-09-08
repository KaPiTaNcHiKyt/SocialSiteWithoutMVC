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
public class MeController(UserService userService, EditUserService editUserService, IHttpContextAccessor context) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<(UserModel, ChatModel)>> GetMe()
    {
        if (context.HttpContext is null)
            return BadRequest();
        var login = context.HttpContext.Request.Cookies["userLogin"];
        if (login is null)
            return BadRequest("Error cookies, authorize again");

        var me = await userService.GetMe(login);

        var result = me.ToModel();

        return Ok(result);
    }
    
    [HttpPatch("[action]")]
    public async Task<IActionResult> EditLogin([Required] string newLogin)
    {
        if (context.HttpContext is null)
            return BadRequest();

        var loginNow = context.HttpContext.Request.Cookies["userLogin"];
        if (loginNow is null)
            return BadRequest("Error cookies, authorize again");
        
        var response = await editUserService.PatchLogin(loginNow, newLogin);
        
        return response ? Ok() : BadRequest();
    }
    
    [HttpPatch("[action]")]
    public async Task<IActionResult> EditPassword([Required] string newPassword)
    {
        if (context.HttpContext is null)
            return BadRequest();

        var loginNow = context.HttpContext.Request.Cookies["userLogin"];
        if (loginNow is null)
            return BadRequest("Error cookies, authorize again");
        
        await editUserService.PatchPassword(loginNow, newPassword);
        
        return Ok();
    }
    
    [HttpPatch("[action]")]
    public async Task<IActionResult> EditNickname([Required] string newNickName)
    {
        if (context.HttpContext is null)
            return BadRequest();

        var loginNow = context.HttpContext.Request.Cookies["userLogin"];
        if (loginNow is null)
            return BadRequest("Error cookies, authorize again");
        
        await editUserService.PatchNickname(loginNow, newNickName);
        
        return Ok();
    }
    
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAccount(string login, string password)
    {
        if (context.HttpContext is null)
            return BadRequest();
        
        context.HttpContext.Response.Cookies.Delete("userLogin");
        context.HttpContext.Response.Cookies.Delete("tasty-cookies");
        
        await userService.Delete(login, password);
        return Ok();
    }
}