using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface IServerService
{
    Result<Server> GetServerById(int id);
    Result<List<Server>> GetServers();
    Result AddServer(ServerForm serverForm);
    Result UpdateServer(int id, ServerForm serverForm);
    Result TerminateServer(int id);
}