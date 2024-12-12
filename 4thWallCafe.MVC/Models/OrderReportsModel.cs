using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

public class OrderReportsModel
{
    public List<OrderReport> OrderReports { get; set; }
    public decimal TotalRevenue { get; set; }
    public OrderReportForm Form { get; set; }
    public SelectList OrderBySelectItems { get; set; }
}

public class OrderReportForm
{
    [DataType(DataType.Date)]
    public DateTime FromDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime ToDate { get; set; }
    public SelectOrderBy OrderBy { get; set; }
}

public static class ReportsUtilities
{
    public static List<OrderBySelectItem> GetSelectList()
    {
        return new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Order Id", Value = 1 },
            new OrderBySelectItem { Text = "Server", Value = 2 },
            new OrderBySelectItem { Text = "Order Total", Value = 3 },
        };
    }
}

public class OrderBySelectItem
{
    public int Value { get; set; }
    public string Text { get; set; }
}