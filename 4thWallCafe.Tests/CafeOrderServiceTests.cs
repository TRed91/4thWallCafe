using _4thWallCafe.App.Services;
using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Tests.MockRepos;

namespace _4thWallCafe.Tests;

public class CafeOrderServiceTests
{
    private CafeOrderService _cafeOrderService = new CafeOrderService(new MockCafeOrderRepo(), new MockItemRepo());
    
    [Test]
    public void AddingOrderItem_UpdatesCafeOrderCorrectly()
    {
        var newCafeOrder = new CafeOrder
        {
            PaymentTypeID = 1,
            ServerID = 1,
            OrderDate = DateTime.Now,
            SubTotal = 0.00m,
            Tip = 2.00m,
            Tax = 0.00m,
            AmountDue = 2.00m,
        };
        var newOrderResult = _cafeOrderService.AddCafeOrder(newCafeOrder);
        
        Assert.That(newOrderResult.Ok, Is.True);
        
        var form = new OrderItemForm
        {
            ItemPriceID = 1,
            Quantity = 2,
        };
        var addItemResult = _cafeOrderService.AddOrderItem(newCafeOrder.OrderID, form);
        
        Assert.That(addItemResult.Ok, Is.True);
        var orderItemResult = _cafeOrderService.GetOrderItem(2);
        Assert.That(orderItemResult.Ok, Is.True);
        Assert.That(orderItemResult.Data.ExtendedPrice, Is.EqualTo(20.00m));
        
        var cafeOrderResult = _cafeOrderService.GetCafeOrder(newCafeOrder.OrderID);
        
        Assert.That(cafeOrderResult.Ok, Is.True);
        Assert.That(cafeOrderResult.Data.SubTotal, Is.EqualTo(20.00m));
        Assert.That(cafeOrderResult.Data.Tax, Is.EqualTo(2.00m));
        Assert.That(cafeOrderResult.Data.AmountDue, Is.EqualTo(24.00m));
    }

    [Test]
    public void UpdatingOrderItem_UpdatesCafeOrderCorrectly()
    {
        var newCafeOrder = new CafeOrder
        {
            PaymentTypeID = 1,
            ServerID = 1,
            OrderDate = DateTime.Now,
            SubTotal = 0.00m,
            Tip = 2.00m,
            Tax = 0.00m,
            AmountDue = 2.00m,
        };
        var newOrderResult = _cafeOrderService.AddCafeOrder(newCafeOrder);
        
        var addForm = new OrderItemForm
        {
            ItemPriceID = 1,
            Quantity = 2,
        };
        var addItemResult = _cafeOrderService.AddOrderItem(newCafeOrder.OrderID, addForm);

        var editForm = new OrderItemForm
        {
            ItemPriceID = 1,
            Quantity = 3,
        };

        var updateItemResult = _cafeOrderService.UpdateOrderItem(2, editForm);
        Assert.That(updateItemResult.Ok, Is.True);
        
        var cafeOrderResult = _cafeOrderService.GetCafeOrder(newCafeOrder.OrderID);
        Assert.That(cafeOrderResult.Ok, Is.True);
        Assert.That(cafeOrderResult.Data.SubTotal, Is.EqualTo(30.00m));
        Assert.That(cafeOrderResult.Data.Tax, Is.EqualTo(3.00m));
        Assert.That(cafeOrderResult.Data.AmountDue, Is.EqualTo(35.00m));
    }

    [Test]
    public void DeleteOrderItem_UpdatesCafeOrderCorrectly()
    {
        var newCafeOrder = new CafeOrder
        {
            PaymentTypeID = 1,
            ServerID = 1,
            OrderDate = DateTime.Now,
            SubTotal = 0.00m,
            Tip = 2.00m,
            Tax = 0.00m,
            AmountDue = 2.00m,
        };
        var newOrderResult = _cafeOrderService.AddCafeOrder(newCafeOrder);
        
        var addForm = new OrderItemForm
        {
            ItemPriceID = 1,
            Quantity = 2,
        };
        var addItemResult = _cafeOrderService.AddOrderItem(newCafeOrder.OrderID, addForm);
        var addForm2 = new OrderItemForm
        {
            ItemPriceID = 2,
            Quantity = 1,
        };
        var addItem2Result = _cafeOrderService.AddOrderItem(newCafeOrder.OrderID, addForm2);
        
        var cafeOrderResult1 = _cafeOrderService.GetCafeOrder(newCafeOrder.OrderID);
        Assert.That(cafeOrderResult1.Data.SubTotal, Is.EqualTo(35.00m));
        Assert.That(cafeOrderResult1.Data.Tax, Is.EqualTo(3.50m));
        Assert.That(cafeOrderResult1.Data.AmountDue, Is.EqualTo(40.50m));
        
        var deleteItemResult = _cafeOrderService.DeleteOrderItem(3);
        Assert.That(deleteItemResult.Ok, Is.True);
        
        var cafeOrderResult2 = _cafeOrderService.GetCafeOrder(newCafeOrder.OrderID);
        Assert.That(cafeOrderResult2.Data.SubTotal, Is.EqualTo(20.00m));
        Assert.That(cafeOrderResult2.Data.Tax, Is.EqualTo(2.00m));
        Assert.That(cafeOrderResult2.Data.AmountDue, Is.EqualTo(24.00m));
    }
}