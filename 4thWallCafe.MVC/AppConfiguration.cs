using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Data.Repositories;
using Serilog.Events;

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
    
    public ICustomerRepository GetCustomerRepository()
    {
        return new CustomerRepository(_config["ConnectionString"]);
    }

    public LogEventLevel GetDbLogEventLevel()
    {
        LogEventLevel level = LogEventLevel.Warning;
        
        switch (_config["Logging:DbLogging:LogLevel"])
        {
            case "Debug":
                level = LogEventLevel.Debug;
                break;
            case "Information":
                level = LogEventLevel.Information;
                break;
            case "Error":
                level = LogEventLevel.Error;
                break;
            case "Warning": 
                level = LogEventLevel.Warning;
                break;
        }
        return level; 
    }
}