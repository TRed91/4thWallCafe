namespace _4thWallCafe.Core.Entities;

public class ItemPrice
{
    public int ItemPriceID { get; set; }
    public int ItemID { get; set; }
    public int TimeOfDayID { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public Item Item { get; set; }
    public TimeOfDay TimeOfDay { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}