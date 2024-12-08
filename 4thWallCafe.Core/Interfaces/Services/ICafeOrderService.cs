using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface ICafeOrderService
{
    Result<List<CafeOrder>> GetCafeOrdersInTimeFrame(DateOnly startDate, DateOnly endDate);
    Result<List<CafeOrder>> GetCafeOrdersByServer(int serverId);
    Result<CafeOrder> GetCafeOrder(int orderId);
    Result AddCafeOrder(CafeOrder cafeOrder);
    Result UpdateCafeOrder(CafeOrder cafeOrder);
    Result DeleteCafeOrder(int orderId);
}