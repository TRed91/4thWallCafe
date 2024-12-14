using _4thWallCafe.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.MVC.Controllers;

public class ManagingController : Controller
{
    private readonly ILogger _logger;
    private readonly IServerService _serverService;
    private readonly IItemService _itemService;

    public ManagingController(
        ILogger<ManagingController> logger, 
        IServerService serverService, 
        IItemService itemService)
    {
        _logger = logger;
        _serverService = serverService;
        _itemService = itemService;
    }
    
    public IActionResult Items()
    {
        var result = _itemService.GetItems();
        
        return View();
    }
}