using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;

namespace _4thWallCafe.Tests.MockRepos;

public class MockServerRepo : IServerRepository
{
    private readonly List<Server> _servers;
    private int id;

    public MockServerRepo()
    {
        _servers = new List<Server>
        {
            new Server
            {
                ServerID = 1, 
                LastName = "Doe", 
                FirstName = "John", 
                DoB = new DateTime(1991, 01, 09), 
                HireDate = DateTime.Today.AddYears(-1), 
                TermDate = null
            },
            new Server
            {
                ServerID = 2, 
                LastName = "Smith", 
                FirstName = "Jane", 
                DoB = new DateTime(1987, 11, 03), 
                HireDate = DateTime.Today.AddYears(-1), 
                TermDate = null
            },
            new Server
            {
                ServerID = 3,
                LastName = "Jones",
                FirstName = "Jane",
                DoB = new DateTime(1987, 11, 06),
                HireDate = DateTime.Today.AddYears(-1),
                TermDate = null
            },
            new Server
            {
                ServerID = 4,
                LastName = "Brown",
                FirstName = "Emily",
                DoB = new DateTime(2000, 06, 06),
                HireDate = DateTime.Today.AddYears(-1),
                TermDate = DateTime.Today.AddDays(-7),
            }
        };
        
        id = _servers.Count;
    }
    public List<Server> GetServers()
    {
        return _servers;
    }

    public Server? GetServerById(int id)
    {
        return _servers.FirstOrDefault(s => s.ServerID == id);
    }

    public void AddServer(Server server)
    {
        server.ServerID = id++;
        _servers.Add(server);
    }

    public void UpdateServer(Server server)
    {
        int index = _servers.FindIndex(s => s.ServerID == server.ServerID);
        _servers[index] = server;
    }
}