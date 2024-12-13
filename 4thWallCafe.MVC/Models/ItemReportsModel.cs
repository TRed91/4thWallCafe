using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Models;
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

public class ItemReportForm
{
    [DataType(DataType.Date)]
    public DateTime FromDate { get; set; } = DateTime.Now;
    [DataType(DataType.Date)]
    public DateTime ToDate { get; set; } = DateTime.Now.AddDays(1);
    public ItemReportsOrderBy OrderBy { get; set; } = ItemReportsOrderBy.ItemName;
}