using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.infrastructureLogic.Services;
using SocialSiteWithoutMVC.Interfaces;
using SocialSiteWithoutMVC.Mapper;
using SocialSiteWithoutMVC.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/UserController")]
[EnableRateLimiting("Default")]
public class UserController(UserService userService, JwtService jwtService, IHttpContextAccessor context) 
    : ControllerBase, ITestings
{
    [EnableRateLimiting("Edit")]
    [HttpPost("PostUser")]
    public async Task<IActionResult> PostUser([Required] string login, [Required] string password, [Required] string nickname)
        => await userService.Add(login, password, nickname) ? Ok() : BadRequest();
    
    
    [HttpPost("Login")]
    public async Task<ActionResult> Login([Required] string login, [Required] string password)
    {
        var user = await userService.Login(login, password);
        if (!user.Item1 || !MainTests()) 
            return BadRequest();
        context.HttpContext!.Response.Cookies.Append("tasty-cookies", user.Item2!);
        return Ok();
    }

    [Authorize]
    [HttpGet("GetUsersByFilter")]
    public async Task<ActionResult<UserModel[]?>> GetUsers([FromServices] IMemoryCache cache, string filter = "") // string.Empty
    {
        if (!MainTests())
            return BadRequest();
        if (cache.TryGetValue($"users_{filter}", out UserModel[]? users))
            return Ok(users);
        var user = await userService.GetByFilter(filter);
        users = user?
            .Select(ModelMapper.UserEntityToModelWithoutChats)
            .OrderBy(u => u.Login)
            .ToArray();
        cache.Set($"users_{filter}", users, TimeSpan.FromMinutes(2));
        return Ok(users);
    }
    
    [Authorize]
    [HttpDelete("Logout")]
    public IActionResult Logout()
    {
        if (!MainTests())
            return BadRequest();
        context.HttpContext!.Response.Cookies.Delete("tasty-cookies");
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