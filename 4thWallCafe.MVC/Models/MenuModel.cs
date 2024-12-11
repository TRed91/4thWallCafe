using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

public class MenuModel
{
    public List<MenuItem> MenuItems { get; set; }
    public SelectList CategoryList { get; set; }
    public SelectList TimeOfDayList { get; set; }
    public List<Category> Categories { get; set; }
    public MenuForm MenuForm { get; set; }
}

public class MenuForm
{
    public int CategoryId { get; set; }
    public int TimeOfDayId { get; set; }
    public string? SearchString { get; set; }
}