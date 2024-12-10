using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.MVC.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        var msg = new TempDataMessage(false, "Fail: Error Message");
        TempDataExtension.Put(TempData, "message", msg);
        return View();
    }
}