using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

/// <summary>
/// This model is passed to the view
/// </summary>
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
    public OrderReportsOrderBy OrderReportsOrderBy { get; set; }
}

public static class ReportsUtilities
{
    /// <summary>
    /// Path to selelctlist instance constructor
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
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

/// <summary>
/// Represents a select option for a selectlist
/// </summary>
public class OrderBySelectItem
{
    public int Value { get; set; }
    public string Text { get; set; }
}