using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.infrastructureLogic.Services;
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
    public async Task<ActionResult<UserModel>> GetMe([FromServices] IMemoryCache cache)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest();
        if (cache.TryGetValue(resultTest.resultCookie!, out UserModel? meModel))
            return Ok(meModel);
        var me = await userService.GetMe(resultTest.resultCookie!);
        if (me == null)
            return NotFound("User not found");
        meModel = ModelMapper.UserEntityToModel(me);
        if (meModel.Chats == null)
            return meModel;
        foreach (var chat in meModel.Chats)
        {
            chat.UsersLogin = chat.UsersLogin.Where(l => l != resultTest.resultCookie!).ToArray();
        }
        cache.Set($"{resultTest.resultCookie}", meModel, TimeSpan.FromMinutes(5));
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
    public async Task<IActionResult> DeleteAccount([Required] string password)
    {
        var resultTest = MainTests("tasty-cookies");
        if (!resultTest.isConfirmTest)
            return BadRequest();
        context.HttpContext!.Response.Cookies.Delete("tasty-cookies");
        await userService.Delete(resultTest.resultCookie!, password);
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