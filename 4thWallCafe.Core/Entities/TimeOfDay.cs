namespace _4thWallCafe.Core.Entities;

public class TimeOfDay
{
    public int TimeOfDayID { get; set; }
    public string TimeOfDayName { get; set; }
    
    public List<ItemPrice> ItemPrices { get; set; }
}