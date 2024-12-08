using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class ServerService : IServerService
{
    public readonly IServerRepository _serverRepository;

    public ServerService(IServerRepository serverRepository)
    {
        _serverRepository = serverRepository;
    }
    
    public Result<Server> GetServerById(int id)
    {
        try
        {
            var server = _serverRepository.GetServerById(id);
            if (server == null)
            {
                return ResultFactory.Fail<Server>("Server not found");
            }

            return ResultFactory.Success(server);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Server>(ex.Message);
        }
    }

    public Result<List<Server>> GetServers()
    {
        try
        {
            var servers = _serverRepository.GetServers();
            return ResultFactory.Success(servers);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Server>>(ex.Message);
        }
    }

    public Result AddServer(Server server)
    {
        if (server.DoB >= DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
        {
            return ResultFactory.Fail("Server must be more than 18 years old");
        }
        try
        {
            _serverRepository.AddServer(server);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result UpdateServer(Server server)
    {
        if (server.DoB >= DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
        {
            return ResultFactory.Fail("Server must be more than 18 years old");
        }

        try
        {
            _serverRepository.UpdateServer(server);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result TerminateServer(int id)
    {
        try
        {
            _serverRepository.TerminateServer(id);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}