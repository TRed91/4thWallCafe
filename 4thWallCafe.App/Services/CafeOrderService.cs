using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class CafeOrderService : ICafeOrderService
{
    private readonly ICafeOrderRepository _cafeOrderRepository;

    public CafeOrderService(ICafeOrderRepository cafeOrderRepository)
    {
        _cafeOrderRepository = cafeOrderRepository;
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
}