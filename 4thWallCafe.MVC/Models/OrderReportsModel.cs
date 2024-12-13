using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Models;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

/// <summary>
/// MVC View model
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

public class OrderReport
{
    public int OrderId { get; set; }
    public string ServerName { get; set; }
    public decimal OrderTotal { get; set; }
}