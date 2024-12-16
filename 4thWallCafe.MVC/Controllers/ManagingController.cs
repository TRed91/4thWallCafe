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
    [ValidateAntiForgeryToken]
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

    public IActionResult Servers(ServerManageForm form)
    {
        var result = _serverService.GetServers();
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
        }
        var servers = result.Data;
        switch (form.OrderBy)
        {
            case ServerManageOrderBy.ServerName:
                servers = servers.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
                break;
            case ServerManageOrderBy.HireDate:
                servers = servers.OrderBy(s => s.HireDate).Reverse().ToList();
                break;
        }

        if (!String.IsNullOrEmpty(form.SearchString))
        {
            string search = form.SearchString.ToLower().Trim();
            servers = servers
                .Where(s => s.LastName.ToLower().Contains(search) || 
                            s.FirstName.ToLower().Contains(search))
                .ToList();
        }

        var model = new ServerManageModel
        {
            Form = form,
            Servers = servers,
            OrderBySelectItems = new SelectList(SelectlistFactory.ServerManageSL(), "Value", "Text")
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult AddServer()
    {
        var form = new ServerForm();
        form.DoB = DateTime.Today;
        return View(form);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddServer(ServerForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }
        var result = _serverService.AddServer(form);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Servers");
        }
        
        _logger.LogInformation($"Server added: {form.LastName}, {form.FirstName}");
        var msg = new TempDataMessage(true, $"Server added: {form.LastName}, {form.FirstName}");
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Servers");
    }

    [HttpGet]
    public IActionResult EditServer(int id)
    {
        var result = _serverService.GetServerById(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Servers");
        }

        var form = new ServerForm
        {
            LastName = result.Data.LastName,
            FirstName = result.Data.FirstName,
            DoB = result.Data.DoB,
        };
        
        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditServer(int id, ServerForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }
        var result = _serverService.UpdateServer(id, form);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Servers");
        }
        
        _logger.LogInformation($"Server updated: {form.LastName}, {form.FirstName}");
        var msg = new TempDataMessage(true, $"Server updated: {form.LastName}, {form.FirstName}");
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Servers");
    }

    [HttpGet]
    public IActionResult TerminateServer(int id)
    {
        var result = _serverService.GetServerById(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Servers");
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TerminateServerPost(int id)
    {
        var result = _serverService.TerminateServer(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var errMsg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", errMsg);
            return RedirectToAction("Servers");
        }
        
        _logger.LogInformation($"Server terminated: {id}");
        var msg = new TempDataMessage(true, $"Server terminated: {id}");
        TempDataExtension.Put(TempData, "message", msg);
        return RedirectToAction("Servers");
    }
}