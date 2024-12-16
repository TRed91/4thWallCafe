using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class ServerService : IServerService
{
    private readonly IServerRepository _serverRepository;

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

    public Result AddServer(ServerForm serverForm)
    {
        if (serverForm.DoB >= DateTime.Now.AddYears(-18))
        {
            return ResultFactory.Fail("Server must be more than 18 years old");
        }

        var server = new Server
        {
            FirstName = serverForm.FirstName,
            LastName = serverForm.LastName,
            DoB = serverForm.DoB,
            HireDate = DateTime.Now,
        };
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

    public Result UpdateServer(int id, ServerForm serverForm)
    {
        if (serverForm.DoB >= DateTime.Now.AddYears(-18))
        {
            return ResultFactory.Fail("Server must be more than 18 years old");
        }
        try
        {
            var server = _serverRepository.GetServerById(id);
            if (server == null)
            {
                return ResultFactory.Fail<Server>("Server not found");
            }
            server.FirstName = serverForm.FirstName;
            server.LastName = serverForm.LastName;
            server.DoB = serverForm.DoB;
            server.TermDate = null;
            
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
            var server = _serverRepository.GetServerById(id);
            if (server == null)
            {
                return ResultFactory.Fail<Server>("Server not found");
            }
            server.TermDate = DateTime.Now;
            _serverRepository.UpdateServer(server);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}