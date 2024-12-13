using System.ComponentModel.DataAnnotations;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

/// <summary>
/// MVC View model
/// </summary>
public class ItemReportsModel
{
    public List<ItemReport> ItemReports { get; set; }
    public ItemReportForm Form { get; set; }
    public SelectList OrderBySelectItems { get; set; }
}

public class ItemReport {
    public string ItemName { get; set; }
    public string CategoryName { get; set; }
    public decimal Revenue { get; set; }
}

public class ItemReportForm
{
    [DataType(DataType.Date)]
    public DateTime FromDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime ToDate { get; set; }
    public ItemReportsOrderBy OrderBy { get; set; }
}