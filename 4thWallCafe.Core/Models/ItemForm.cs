namespace _4thWallCafe.Core.Models;

public class ItemForm
{
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public int CategoryId { get; set; }
    
    public List<PriceForm> PriceFields { get; set; }
}

public class PriceForm
{
    public int TimeOfDayId { get; set; }
    public decimal Price { get; set; }
}