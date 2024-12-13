using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;
using _4thWallCafe.MVC.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly IItemService _itemService;
    private readonly ILogger _logger;

    public MenuController(IItemService itemService, ILogger<MenuController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }
    
    public IActionResult Index(MenuModel? form = null)
    {
        int categoryId = form?.MenuForm?.CategoryId ?? 0;
        int timeOfDayId = form?.MenuForm?.TimeOfDayId ?? 1;
        string searchString = form?.MenuForm?.SearchString ?? "";
        
        var items = new List<MenuItem>();
        Result<List<MenuItem>> result;
        
        if (categoryId == 0)
        {
            result = _itemService.GetItemByTimeOfDay(timeOfDayId);
        }
        else
        {
            result = _itemService.GetItemsByCategoryAndTimeOfDay(categoryId, timeOfDayId);
        }

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Index", "Home");
        }
        
        items = result.Data;

        if (searchString != null)
        {
            items = items
                .Where(i => i.ItemName.ToLower().Contains(searchString.ToLower()))
                .ToList();
        }
        
        var categoriesResult = _itemService.GetCategories();
        if (!categoriesResult.Ok)
        {
            _logger.LogError(categoriesResult.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Index", "Home");
        }
        var timeOfDaysResult = _itemService.GetTimeOfDays();
        if (!timeOfDaysResult.Ok)
        {
            _logger.LogError(timeOfDaysResult.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Index", "Home");
        }

        var categories = categoryId == 0 ? 
            categoriesResult.Data : 
            categoriesResult.Data.Where(c => c.CategoryID == categoryId).ToList();
        
        var model = new MenuModel
        {
            MenuItems = items,
            CategoryList = new SelectList(categoriesResult.Data, "CategoryID", "CategoryName"),
            TimeOfDayList = new SelectList(timeOfDaysResult.Data, "TimeOfDayID", "TimeOfDayName"),
            Categories = categories,
            MenuForm = new MenuForm
            {
                CategoryId = categoryId,
                TimeOfDayId = timeOfDayId,
                SearchString = searchString
            }
        };
        
        return View(model);
    }
}