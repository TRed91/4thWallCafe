namespace _4thWallCafe.Core.Entities;

public class ApiOrder
{
    public int ApiOrderID { get; set; }
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    
    public Customer Customer { get; set; }
    public CafeOrder Order { get; set; }
}