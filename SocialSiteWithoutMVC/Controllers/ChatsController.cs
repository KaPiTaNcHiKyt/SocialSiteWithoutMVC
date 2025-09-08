using Microsoft.AspNetCore.Mvc;

namespace SocialSiteWithoutMVC.Controllers;

public class ChatsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}