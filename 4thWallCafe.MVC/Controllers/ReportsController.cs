using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.MVC.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Controllers;

[Authorize(Roles = "Admin, Accountant")]
public class ReportsController : Controller
{
    private readonly ICafeOrderService _cafeOrderService;
    private readonly IItemService _itemService;
    private readonly ILogger _logger;

    public ReportsController(
        ICafeOrderService cafeOrderService,  
        IItemService itemService,
        ILogger<ReportsController> logger)
    {
        _cafeOrderService = cafeOrderService;
        _itemService = itemService;
        _logger = logger;
    }

    public IActionResult Orders(OrderReportForm form)
    {
        var result = _cafeOrderService.GetCafeOrdersInTimeFrame(
            DateOnly.FromDateTime(form.FromDate), 
            DateOnly.FromDateTime(form.ToDate));

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Index", "Home");
        }

        var orderReports = result.Data.Select(o => new OrderReport
        {
            OrderId = o.OrderID,
            ServerName = o.Server.LastName + ", " + o.Server.FirstName,
            OrderTotal = o.AmountDue
        }).ToList();

        var orderBy = form?.OrderReportsOrderBy ?? OrderReportsOrderBy.OrderID;
        switch (orderBy)
        {
            case OrderReportsOrderBy.OrderID:
                orderReports = orderReports.OrderBy(o => o.OrderId).ToList();
                break;
            case OrderReportsOrderBy.Server:
                orderReports = orderReports.OrderBy(o => o.ServerName).ToList();
                break;
            case OrderReportsOrderBy.OrderTotal:
                orderReports = orderReports.OrderBy(o => o.OrderTotal).Reverse().ToList();
                break;
        }

        var model = new OrderReportsModel
        {
            OrderReports = orderReports,
            TotalRevenue = orderReports.Sum(o => o.OrderTotal),
            OrderBySelectItems = new SelectList(
                ReportsUtilities.OrderReportSL(), "Value", "Text"),
            Form = new OrderReportForm()
        };
        
        return View(model);
    }

    public IActionResult Items(ItemReportForm form)
    {
        var result = _itemService.GetItemReports(
            DateOnly.FromDateTime(form.FromDate), 
            DateOnly.FromDateTime(form.ToDate));

        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Index", "Home");
        }

        List<ItemReport> itemReports = result.Data;
        switch (form.OrderBy)
        {
            case ItemReportsOrderBy.Revenue:
                itemReports = itemReports.OrderBy(o => o.Revenue).Reverse().ToList();
                break;
            case ItemReportsOrderBy.ItemName:
                itemReports = itemReports.OrderBy(o => o.ItemName).ToList();
                break;
            case ItemReportsOrderBy.CategoryName:
                itemReports = itemReports.OrderBy(o => o.CategoryName).ToList();
                break;
        }
        {
            
        }

        var model = new ItemReportsModel
        {
            ItemReports = itemReports,
            Form = form ?? new ItemReportForm(),
            OrderBySelectItems = new SelectList(
                ReportsUtilities.ItemReportSL(), "Value", "Text"),
        };
        
        return View(model);
    }

    public IActionResult OrderDetail(int id)
    {
        var result = _cafeOrderService.GetCafeOrder(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            var msg = new TempDataMessage(false, result.Message);
            TempDataExtension.Put(TempData, "message", msg);
            return RedirectToAction("Orders");
        }
        
        return View(result.Data);
    }
}