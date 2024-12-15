using _4thWallCafe.Core.Entities;
using _4thWallCafe.MVC.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

public class ItemManageModel
{
    public List<Item> Items { get; set; }
    public SelectList OrderBySelectItems { get; set; }
    public ItemManageForm Form { get; set; }
}

public class ItemManageForm
{
    public ItemManageOrderBy OrderBy { get; set; } = ItemManageOrderBy.ItemName;
    public string SearchString { get; set; } = "";
}