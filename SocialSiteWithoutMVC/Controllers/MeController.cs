using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MyController(UserService service, IHttpContextAccessor context) : ControllerBase
{
    [HttpPatch("[action]")]
    public async Task<IActionResult> PatchData(ChatModel chatModel)
    {
        
    }
    
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAccount(string login, string password)
    {
        await service.Delete(login, password);
        return Ok();
    }
}