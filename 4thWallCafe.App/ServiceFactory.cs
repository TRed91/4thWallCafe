using _4thWallCafe.App.Services;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;

namespace _4thWallCafe.App;

public class ServiceFactory
{
    private readonly IAppConfiguration _config;

    public ServiceFactory(IAppConfiguration config)
    {
        _config = config;
    }
    
    public IServerService GenerateServerService()
    {
        return new ServerService(_config.GetServerRepository());
    }

    public IItemService GenerateItemService()
    {
        return new ItemService(_config.GetItemRepository());
    }

    public ICafeOrderService GenerateCafeOrderService()
    {
        return new CafeOrderService(_config.GetCafeOrderRepository());
    }

    public ICustomerService GenerateCustomerService()
    {
        return new CustomerService(_config.GetCustomerRepository());
    }
}