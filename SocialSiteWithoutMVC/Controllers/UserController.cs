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
public class UserController(UserService userService, JwtService jwtService, ILogger<UserController> logger, IHttpContextAccessor context) 
    : ControllerBase, ITestings
{
    [HttpPost("[action]")]
    public async Task<IActionResult> PostUser([Required] string login, [Required] string password, [Required] string nickname)
        => await userService.Add(login, password, nickname) ? Ok() : BadRequest();
    
    [HttpPost("[action]")]
    public async Task<ActionResult> Login([Required] string login, [Required] string password)
    {
        var user = await userService.Login(login, password);

        if (!user.Item1 || context.HttpContext is null) 
            return BadRequest();

        context.HttpContext.Response.Cookies.Append("tasty-cookies", user.Item2!);

        return Ok();
    }

    [Authorize]
    [HttpGet("[action]")]
    public async Task<ActionResult<UserModel[]?>> GetUsers(string filter = "") // string.Empty
    {
        if (!MainTests())
            return BadRequest();
        
        var user = await userService.GetByFilter(filter);
        
        return Ok(user?
            .Select(u => u.ToModel())
            .OrderBy(u => u.Login)
            .ToArray());
    }
    
    [Authorize]
    [HttpDelete("[action]")]
    public IActionResult Logout()
    {
        if (!MainTests())
            return BadRequest();
        
        context.HttpContext!.Response.Cookies.Delete("tasty-cookies");
        return Ok();
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