using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface ICafeOrderService
{
    Result<List<CafeOrder>> GetCafeOrdersInTimeFrame(DateOnly startDate, DateOnly endDate);
    Result<List<CafeOrder>> GetCafeOrdersByServer(int serverId);
    Result<CafeOrder> GetCafeOrder(int orderId);
    Result<OrderItem> GetOrderItem(int orderItemId);
    Result AddCafeOrder(CafeOrder cafeOrder);
    Result AddOrderItem(int cafeOrderId, OrderItemForm form);
    Result UpdateCafeOrder(CafeOrder cafeOrder);
    Result UpdateOrderItem(int orderItemId, OrderItemForm form);
    Result DeleteCafeOrder(int orderId);
    Result DeleteOrderItem(int orderItemId);
}