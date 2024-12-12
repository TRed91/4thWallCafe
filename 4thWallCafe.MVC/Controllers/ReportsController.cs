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
    private readonly IServerService _serverService;
    private readonly IItemService _itemService;
    private readonly ILogger _logger;

    public ReportsController(
        ICafeOrderService cafeOrderService, 
        IServerService serverService, 
        IItemService itemService,
        ILogger<ReportsController> logger)
    {
        _cafeOrderService = cafeOrderService;
        _serverService = serverService;
        _itemService = itemService;
        _logger = logger;
    }

    public IActionResult Orders(OrderReportForm? form = null)
    {
        var fromDate = form?.FromDate ?? DateTime.Now;
        var toDate = form?.ToDate ?? DateTime.Now.AddDays(1);
        
        var result = _cafeOrderService.GetCafeOrdersInTimeFrame(
            DateOnly.FromDateTime(fromDate), 
            DateOnly.FromDateTime(toDate));

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

        var orderBy = form?.OrderBy ?? SelectOrderBy.OrderID;
        switch (orderBy)
        {
            case SelectOrderBy.OrderID:
                orderReports = orderReports.OrderBy(o => o.OrderId).ToList();
                break;
            case SelectOrderBy.Server:
                orderReports = orderReports.OrderBy(o => o.ServerName).ToList();
                break;
            case SelectOrderBy.OrderTotal:
                orderReports = orderReports.OrderBy(o => o.OrderTotal).ToList();
                break;
        }

        var model = new OrderReportsModel
        {
            OrderReports = orderReports,
            TotalRevenue = orderReports.Sum(o => o.OrderTotal),
            OrderBySelectItems = new SelectList(
                ReportsUtilities.GetSelectList(), "Value", "Text"),
            Form = new OrderReportForm()
        };
        
        return View(model);
    }
}