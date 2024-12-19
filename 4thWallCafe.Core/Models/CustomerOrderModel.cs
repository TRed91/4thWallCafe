using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Models;

public class CustomerOrderModel
{
    public int CustomerOrderID { get; set; }
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    
    public CustomerModel Customer { get; set; }
    public CafeOrder Order { get; set; }
}