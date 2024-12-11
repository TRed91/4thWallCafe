using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Models;

public class MenuItem
{
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }

    public MenuItem() { }

    public MenuItem(ItemPrice item)
    {
        ItemName = item.Item.ItemName;
        ItemDescription = item.Item.ItemDescription;
        Category = item.Item.Category.CategoryName;
        Price = item.Price;
    }
}