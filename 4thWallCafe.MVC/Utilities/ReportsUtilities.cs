namespace _4thWallCafe.MVC.Utilities;

public static class ReportsUtilities
{
    /// <summary>
    /// Generates a SelectList for Order Reports View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static List<OrderBySelectItem> OrderReportSL()
    {
        return new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Order Id", Value = 1 },
            new OrderBySelectItem { Text = "Server", Value = 2 },
            new OrderBySelectItem { Text = "Order Total", Value = 3 },
        };
    }

    /// <summary>
    /// Generates a SelectList for Item Reports View
    /// </summary>
    /// <returns>List of OrderBy Select Options</returns>
    public static List<OrderBySelectItem> ItemReportSL()
    {
        return new List<OrderBySelectItem>
        {
            new OrderBySelectItem { Text = "Item", Value = 1 },
            new OrderBySelectItem { Text = "Category", Value = 2 },
            new OrderBySelectItem { Text = "Revenue", Value = 3 },
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