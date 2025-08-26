using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.Extensions;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly SocialSiteDbContext _context;

    public UserController(SocialSiteDbContext context, ILogger<UserController> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> PostUser(string login, string password, string nickname)
    {
        var success = await new BusinessLogic.Services.UserService(_context).AddUser(login, password, nickname);
        if (!success) return BadRequest();
        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult<UserModel>> GetUserByLoginAndPassword(string login, string password)
    {
        var service = new BusinessLogic.Services.UserService(_context);
        var user = await service.GetUserByPassword(login, password);
        
        if (user == null) return BadRequest();

        return user.ToModel();
    }
}