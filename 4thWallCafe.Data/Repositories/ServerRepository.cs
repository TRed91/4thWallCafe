using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace _4thWallCafe.Data.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly string _connectionString;

    public ServerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public List<Server> GetServers()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = "SELECT * FROM [Server] Where TermDate IS NULL";
            return cn.Query<Server>(sql).ToList();
        }
    }

    public Server? GetServerById(int id)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = "SELECT * FROM [Server] WHERE ServerID = @Id";
            return cn.QueryFirstOrDefault<Server>(sql, new { Id = id });
        }
    }

    public void AddServer(Server server)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO [Server] (FirstName, LastName, HireDate, TermDate, DoB) 
                        VALUES (@FirstName, @LastName, @HireDate, @TermDate, @Dob);
                        SELECT Scope_Identity();";
            
            var param = new
            {
                server.FirstName,
                server.LastName,
                server.HireDate,
                TermDate = (DateTime?)null,
                server.DoB
            };
            
            server.ServerID = cn.ExecuteScalar<int>(sql, param);
        }
    }

    public void UpdateServer(Server server)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE [Server] SET 
                    FirstName = @FirstName,
                    LastName = @LastName,
                    TermDate = @TermDate,
                    DoB = @DoB
                    WHERE ServerID = @ServerID;";
            var p = new
            {
                server.FirstName,
                server.LastName,
                server.TermDate,
                server.DoB,
                server.ServerID
            };
            cn.Execute(sql, p);
        }
    }
}