using Serilog.Events;

namespace _4thWallCafe.Core.Interfaces;

public interface IAppConfiguration
{
    IServerRepository GetServerRepository();
    IItemRepository GetItemRepository();
    ICafeOrderRepository GetCafeOrderRepository();
    ICustomerRepository GetCustomerRepository();
    LogEventLevel GetDbLogEventLevel();
}