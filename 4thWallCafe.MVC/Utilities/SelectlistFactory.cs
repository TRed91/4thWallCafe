using _4thWallCafe.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Utilities;

/// <summary>
/// Contains various methods to generate SelectLists for view models
/// </summary>
public static class SelectlistFactory
{
    /// <summary>
    /// Generates a SelectList for Order Reports View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static SelectList OrderReportSL()
    {
        var list = new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Order Id", Value = 1 },
            new OrderBySelectItem { Text = "Server", Value = 2 },
            new OrderBySelectItem { Text = "Order Total", Value = 3 },
        };
        
        return new SelectList(list, "Value", "Text");
    }

    /// <summary>
    /// Generates a SelectList for Item Reports View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static SelectList ItemReportSL()
    {
        var list = new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Item", Value = 1 },
            new OrderBySelectItem { Text = "Category", Value = 2 },
            new OrderBySelectItem { Text = "Revenue", Value = 3 },
        };
        
        return new SelectList(list, "Value", "Text");
    }

    /// <summary>
    /// Generates a SelectList for Item Management View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static SelectList ItemManageSL()
    {
        var list = new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Item", Value = 1 },
            new OrderBySelectItem { Text = "Category", Value = 2 },
        };
        
        return new SelectList(list, "Value", "Text");
    }

    /// <summary>
    /// Generates a SelectList for Server Management View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static SelectList ServerManageSL()
    {
        var list = new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Name", Value = 1 },
            new OrderBySelectItem { Text = "Hire Date", Value = 2 },
        };
        
        return new SelectList(list, "Value", "Text");
    }

    /// <summary>
    /// Generates a Categories SelectList
    /// </summary>
    /// <param name="categories">List of categories</param>
    /// <returns>SelectList</returns>
    public static SelectList CategorySL(List<Category> categories)
    {
        return new SelectList(categories, "CategoryID", "CategoryName");
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