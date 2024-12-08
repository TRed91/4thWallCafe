using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface IServerService
{
    Result<Server> GetServerById(int id);
    Result<List<Server>> GetServers();
    Result AddServer(Server server);
    Result UpdateServer(Server server);
    Result TerminateServer(int id);
}