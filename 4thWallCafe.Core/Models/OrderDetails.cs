namespace _4thWallCafe.Core.Models;

public class OrderDetails
{
    public int OrderID { get; set; }
    public string ServerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentTypeName { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Tip { get; set; }
    public decimal AmountDue { get; set; }
    
    public List<OrderDetailsItem> Items { get; set; }
}