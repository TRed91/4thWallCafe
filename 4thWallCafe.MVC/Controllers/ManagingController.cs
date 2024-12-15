using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.MVC.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Controllers;

[Authorize(Roles = "Admin, Manager")]
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
    
    public IActionResult Items(ItemManageForm form)
    {
        var result = _itemService.GetItems();

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            RedirectToAction("Index", "Home");
        }

        var items = result.Data;
        // Reduce results to search string value
        if (!String.IsNullOrEmpty(form.SearchString) && !String.IsNullOrWhiteSpace(form.SearchString))
        {
            items = items
                .Where(i => i.ItemName
                    .ToLower()
                    .Contains(form.SearchString.Trim().ToLower()))
                .ToList();   
        }

        // order list by "order by" value
        switch (form.OrderBy)
        {
            case ItemManageOrderBy.ItemName:
                items = items.OrderBy(i => i.ItemName).ToList();
                break;
            case ItemManageOrderBy.CategoryName:
                items = items.OrderBy(i => i.Category.CategoryName).ToList();
                break;
        }
        
        var model = new ItemManageModel
        {
            Items = items,
            Form = form,
            OrderBySelectItems = new SelectList(SelectlistFactory.ItemManageSL(), "Value", "Text")
        };
        
        return View(model);
    }

    [HttpGet]
    public IActionResult AddItem()
    {
        var categoriesResult = _itemService.GetCategories();
        if (!categoriesResult.Ok)
        {
            _logger.LogError(categoriesResult.Message);
            var msg = new TempDataMessage(false, categoriesResult.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Items");
        }
        var model = new AddItemModel
        {
            Form = new ItemForm(),
            CategoryList = new SelectList(categoriesResult.Data, "CategoryID", "CategoryName"),
        };
        
        return View(model);
    }

    [HttpPost]
    public IActionResult AddItem(AddItemModel model)
    {
        
        if (!ModelState.IsValid)
        {
            var categoriesResult = _itemService.GetCategories();
            if (!categoriesResult.Ok)
            {
                _logger.LogError(categoriesResult.Message);
                var errMsg = new TempDataMessage(false, categoriesResult.Message);
                TempDataExtension.Put(TempData, "message", errMsg);
                return RedirectToAction("Items");
            }
            model.CategoryList = new SelectList(categoriesResult.Data, "CategoryID", "CategoryName");
            return View(model);
        }
        
        var addItem = new AddItem();
        addItem.ItemName = model.Form.ItemName;
        addItem.ItemDescription = model.Form.ItemDescription;
        addItem.CategoryId = model.Form.CategoryId;
        addItem.PriceFields = new List<PriceField>();
        
        if (model.Form.BreakfastPrice != null)
        {
            addItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.BreakfastPrice, 
                TimeOfDayId = 1
            });
        }
        if (model.Form.LunchPrice != null)
        {
            addItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.LunchPrice, 
                TimeOfDayId = 2
            });
        }
        if (model.Form.HappyHourPrice != null)
        {
            addItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.HappyHourPrice, 
                TimeOfDayId = 3
            });
        }
        if (model.Form.DinnerPrice != null)
        {
            addItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.DinnerPrice, 
                TimeOfDayId = 4
            });
        }
        if (model.Form.SeasonalPrice != null)
        {
            addItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.SeasonalPrice, 
                TimeOfDayId = 5
            });
        }
        
        var result = _itemService.AddItem(addItem);

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Items");
        }
        
        _logger.LogInformation("Item added: " + addItem.ItemName);
        var msg = new TempDataMessage(true, "Item added: " + addItem.ItemName);
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Items");
    }

    public IActionResult EditItem(int id)
    {
        var itemResult = _itemService.GetItemById(id);
        if (!itemResult.Ok)
        {
            _logger.LogError(itemResult.Message);
            var msg = new TempDataMessage(false, itemResult.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Items");
        }
        var categoriesResult = _itemService.GetCategories();
        if (!categoriesResult.Ok)
        {
            _logger.LogError(categoriesResult.Message);
            var msg = new TempDataMessage(false, categoriesResult.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Items");
        }
        
        var form = new ItemForm();
        form.ItemName = itemResult.Data.ItemName;
        form.ItemDescription = itemResult.Data.ItemDescription;
        form.CategoryId = itemResult.Data.CategoryID;
        form.BreakfastPrice = itemResult.Data.ItemPrices
            .FirstOrDefault(p => p.TimeOfDayID == 1)?.Price;
        form.LunchPrice = itemResult.Data.ItemPrices
            .FirstOrDefault(p => p.TimeOfDayID == 2)?.Price;
        form.HappyHourPrice = itemResult.Data.ItemPrices
            .FirstOrDefault(p => p.TimeOfDayID == 3)?.Price;
        form.DinnerPrice = itemResult.Data.ItemPrices
            .FirstOrDefault(p => p.TimeOfDayID == 4)?.Price;
        form.SeasonalPrice = itemResult.Data.ItemPrices
            .FirstOrDefault(p => p.TimeOfDayID == 5)?.Price;
        
        var model = new AddItemModel
        {
            Form = form,
            CategoryList = new SelectList(categoriesResult.Data, "CategoryID", "CategoryName"),
        };
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditItem(int id, AddItemModel model)
    {
        if (!ModelState.IsValid)
        {
            var categoriesResult = _itemService.GetCategories();
            if (!categoriesResult.Ok)
            {
                _logger.LogError(categoriesResult.Message);
                var errMsg = new TempDataMessage(false, categoriesResult.Message);
                TempDataExtension.Put(TempData, "message", errMsg);
                return RedirectToAction("Items");
            }
            model.CategoryList = new SelectList(categoriesResult.Data, "CategoryID", "CategoryName");
            return View(model);
        }
        
        var updateItem = new AddItem();
        updateItem.ItemName = model.Form.ItemName;
        updateItem.ItemDescription = model.Form.ItemDescription;
        updateItem.CategoryId = model.Form.CategoryId;
        updateItem.PriceFields = new List<PriceField>();
        
        if (model.Form.BreakfastPrice != null)
        {
            updateItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.BreakfastPrice, 
                TimeOfDayId = 1
            });
        }
        if (model.Form.LunchPrice != null)
        {
            updateItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.LunchPrice, 
                TimeOfDayId = 2
            });
        }
        if (model.Form.HappyHourPrice != null)
        {
            updateItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.HappyHourPrice, 
                TimeOfDayId = 3
            });
        }
        if (model.Form.DinnerPrice != null)
        {
            updateItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.DinnerPrice, 
                TimeOfDayId = 4
            });
        }
        if (model.Form.SeasonalPrice != null)
        {
            updateItem.PriceFields.Add(new PriceField
            {
                Price = (decimal)model.Form.SeasonalPrice, 
                TimeOfDayId = 5
            });
        }
        
        var result = _itemService.UpdateItem(id, updateItem);

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Items");
        }
        
        _logger.LogInformation("Item updated: " + updateItem.ItemName);
        var msg = new TempDataMessage(true, "Item updated: " + updateItem.ItemName);
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Items");
    }

    [HttpGet]
    public IActionResult DeleteItem(int id)
    {
        var result = _itemService.GetItemById(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Items");
        }
        var model = new DeleteItemModel(result.Data);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteItemPost(int id)
    {
        var result = _itemService.DeleteItem(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Items");
        }
        
        _logger.LogInformation("Item deleted: " + id);
        var msg = new TempDataMessage(true, "Item deleted: " + id);
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Items");
    }
}