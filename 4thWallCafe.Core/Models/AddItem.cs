namespace _4thWallCafe.Core.Models;

public class AddItem
{
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public int CategoryId { get; set; }
    
    public List<PriceField> PriceFields { get; set; }
}

public class PriceField
{
    public int TimeOfDayId { get; set; }
    public decimal Price { get; set; }
}