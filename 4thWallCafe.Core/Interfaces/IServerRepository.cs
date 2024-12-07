using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Interfaces;

public interface IServerRepository
{
    List<Server> GetServers();
    Server? GetServerById(int id);
    void AddServer(Server server);
    void UpdateServer(Server server);
    void TerminateServer(int id);
}