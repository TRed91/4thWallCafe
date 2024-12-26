using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.MVC.Models;

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

    public OrderDetails() { }

    public OrderDetails(CafeOrder order)
    {
        OrderID = order.OrderID;
        ServerName = order.Server.LastName + ", " + order.Server.FirstName;
        OrderDate = order.OrderDate;
        PaymentTypeName = order.PaymentType.PaymentTypeName;
        SubTotal = order.SubTotal;
        Tax = order.Tax;
        Tip = order.Tip;
        AmountDue = order.AmountDue;
        
        Items = order.OrderItems
            .Select(i => new OrderDetailsItem(i))
            .ToList();
    }
}

public class OrderDetailsItem
{
    public string Name { get; set; }
    public byte Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderDetailsItem() { }

    public OrderDetailsItem(OrderItem orderItem)
    {
        Name = orderItem.ItemPrice.Item.ItemName;
        Quantity = orderItem.Quantity;
        Price = orderItem.ExtendedPrice;
    }
}