using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class CafeOrderService : ICafeOrderService
{
    private readonly ICafeOrderRepository _cafeOrderRepository;
    private readonly IItemRepository _itemRepository;

    public CafeOrderService(ICafeOrderRepository cafeOrderRepository, IItemRepository itemRepository)
    {
        _cafeOrderRepository = cafeOrderRepository;
        _itemRepository = itemRepository;
    }
    
    public Result<List<CafeOrder>> GetCafeOrdersInTimeFrame(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var orders = _cafeOrderRepository
                .GetCafeOrdersInTimeframe(startDate, endDate);
            return ResultFactory.Success(orders);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CafeOrder>>(ex.Message);
        }
    }

    public Result<List<CafeOrder>> GetCafeOrdersByServer(int serverId)
    {
        try
        {
            var orders = _cafeOrderRepository.GetCafeOrdersByServer(serverId);
            return ResultFactory.Success(orders);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CafeOrder>>(ex.Message);
        }
    }

    public Result<CafeOrder> GetCafeOrder(int orderId)
    {
        try
        {
            var order = _cafeOrderRepository.GetCafeOrder(orderId);
            if (order == null)
            {
                return ResultFactory.Fail<CafeOrder>("Order not found");
            }

            return ResultFactory.Success(order);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<CafeOrder>(ex.Message);
        }
    }

    public Result<OrderItem> GetOrderItem(int orderItemId)
    {
        try
        {
            var oderItem = _cafeOrderRepository.GetOrderItem(orderItemId);
            if (oderItem == null)
            {
                return ResultFactory.Fail<OrderItem>("Order Item not found");
            }
            return ResultFactory.Success(oderItem);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<OrderItem>(ex.Message);
        }
    }

    public Result AddCafeOrder(CafeOrder cafeOrder)
    {
        try
        {
            _cafeOrderRepository.AddCafeOrder(cafeOrder);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result AddOrderItem(int cafeOrderId, OrderItemForm form)
    {
        try
        {
            // get ItemPrice and CafeOrder
            var itemPrice = _itemRepository.GetItemPriceById(form.ItemPriceID);
            
            if (itemPrice == null)
            {
                return ResultFactory.Fail("Item price not found");
            }

            // create the OrderItem record
            var orderItem = new OrderItem
            {
                OrderID = cafeOrderId,
                ItemPriceID = form.ItemPriceID,
                ExtendedPrice = itemPrice.Price * form.Quantity,
                Quantity = form.Quantity,
            };
            _cafeOrderRepository.AddOrderItem(orderItem);

            // Update the CafeOrder with 10% Tax
            var cafeOrder = _cafeOrderRepository.GetCafeOrder(cafeOrderId);
            if (cafeOrder == null)
            {
                return ResultFactory.Fail("Order not found");
            }
            var sum = cafeOrder.OrderItems.Sum(x => x.ExtendedPrice);
            
            cafeOrder.SubTotal = sum;
            cafeOrder.Tax = sum / 10;
            cafeOrder.AmountDue = sum + (sum / 10) + cafeOrder.Tip;
            
            _cafeOrderRepository.EditCafeOrder(cafeOrder);
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result UpdateCafeOrder(CafeOrder cafeOrder)
    {
        try
        {
            _cafeOrderRepository.EditCafeOrder(cafeOrder);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result UpdateOrderItem(int orderItemId, OrderItemForm form)
    {
        try
        {
            var orderItem = _cafeOrderRepository.GetOrderItem(orderItemId);
            if (orderItem == null)
            {
                return ResultFactory.Fail("Order item not found");
            }
            
            var itemPrice = _itemRepository.GetItemPriceById(form.ItemPriceID);
            if (itemPrice == null)
            {
                return ResultFactory.Fail("Item price not found");
            }
            
            // Update the OrderItem
            orderItem.ItemPriceID = form.ItemPriceID;
            orderItem.Quantity = form.Quantity;
            orderItem.ExtendedPrice = itemPrice.Price * form.Quantity;
            
            _cafeOrderRepository.EditOrderItem(orderItem);
            
            // Update the CafeOrder data
            var cafeOrder = _cafeOrderRepository.GetCafeOrder(orderItem.OrderID);
            if (cafeOrder == null)
            {
                return ResultFactory.Fail("Order not found");
            }
            cafeOrder.SubTotal = cafeOrder.OrderItems.Sum(x => x.ExtendedPrice);
            cafeOrder.Tax = cafeOrder.SubTotal / 10;
            cafeOrder.AmountDue = cafeOrder.SubTotal + cafeOrder.Tax + cafeOrder.Tip;
            _cafeOrderRepository.EditCafeOrder(cafeOrder);
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteCafeOrder(int orderId)
    {
        try
        {
            _cafeOrderRepository.DeleteCafeOrder(orderId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteOrderItem(int orderItemId)
    {
        try
        {
            // Get the order Item before deletion
            var orderItem = _cafeOrderRepository.GetOrderItem(orderItemId);
            if (orderItem == null)
            {
                return ResultFactory.Fail("Order item not found");
            }
            
            // Delete the OrderItem
            _cafeOrderRepository.DeleteOrderItem(orderItemId);
            
            // update the cafeOrder with the new price
            var cafeOrder = _cafeOrderRepository.GetCafeOrder(orderItem.OrderID);
            cafeOrder.SubTotal = cafeOrder.OrderItems.Sum(x => x.ExtendedPrice);
            cafeOrder.Tax = cafeOrder.SubTotal / 10;
            cafeOrder.AmountDue = cafeOrder.SubTotal + cafeOrder.Tax + cafeOrder.Tip;
            
            _cafeOrderRepository.EditCafeOrder(cafeOrder);
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}