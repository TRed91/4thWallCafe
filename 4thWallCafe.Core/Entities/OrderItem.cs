﻿namespace _4thWallCafe.Core.Entities;

public class OrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int ItemPriceID { get; set; }
    public byte Quantity { get; set; }
    public decimal ExtendedPrice { get; set; }
    
    public ItemPrice ItemPrice { get; set; }
    public CafeOrder Order { get; set; }
}