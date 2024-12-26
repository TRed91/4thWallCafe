using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;

namespace _4thWallCafe.Core.Interfaces;

public interface ICafeOrderRepository
{
    List<CafeOrder> GetCafeOrdersInTimeframe(DateOnly startDate, DateOnly endDate);
    List<CafeOrder> GetCafeOrdersByServer(int serverId);
    CafeOrder? GetCafeOrder(int orderId);
    OrderItem? GetOrderItem(int orderItemId);
    void AddCafeOrder(CafeOrder cafeOrder);
    void AddOrderItem(OrderItem orderItem);
    void EditCafeOrder(CafeOrder cafeOrder);
    void EditOrderItem(OrderItem orderItem);
    void DeleteCafeOrder(int orderId);
    void DeleteOrderItem(int orderItemId);
}