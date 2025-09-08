using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.Extensions;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service, ILogger<UserController> logger, IHttpContextAccessor context) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> PostUser([Required] string login, [Required] string password, [Required] string nickname)
        => await service.Add(login, password, nickname) ? Ok() : BadRequest();
    
    [HttpPost("[action]")]
    public async Task<ActionResult> Login([Required] string login, [Required] string password)
    {
        var user = await service.Login(login, password);

        if (!user.Item1 || context.HttpContext is null) 
            return BadRequest();

        context.HttpContext.Response.Cookies.Append("tasty-cookies", user.Item2!);
        context.HttpContext.Response.Cookies.Append("userLogin", login);

        return Ok();
    }

    [Authorize]
    [HttpGet("[action]")]
    public async Task<ActionResult<UserModel[]?>> GetUsers(string filter = "") // string.Empty
    {
        if (context.HttpContext is null)
            return BadRequest();
        
        var user = await service.GetByFilter(filter);
        
        return Ok(user?
            .Select(u => u.ToModel())
            .OrderBy(u => u.Login)
            .ToArray());
    }
    
    [Authorize]
    [HttpDelete("[action]")]
    public IActionResult Logout()
    {
        if (context.HttpContext is null)
            return BadRequest();
        
        context.HttpContext.Response.Cookies.Delete("userLogin");
        context.HttpContext.Response.Cookies.Delete("tasty-cookies");
        return Ok();
    }
}