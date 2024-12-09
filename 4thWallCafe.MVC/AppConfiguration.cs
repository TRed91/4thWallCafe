using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Data.Repositories;

namespace _4thWallCafe.MVC;

public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _config;

    public AppConfiguration(IConfiguration config)
    {
        _config = config;
    }
    public IServerRepository GetServerRepository()
    {
        return new ServerRepository(_config["ConnectionString"]);
    }

    public IItemRepository GetItemRepository()
    {
        return new ItemRepository(_config["ConnectionString"]);
    }

    public ICafeOrderRepository GetCafeOrderRepository()
    {
        return new CafeOrderRepository(_config["ConnectionString"]);
    }
}