using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Models;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;

namespace _4thWallCafe.Data.Repositories;

public class CafeOrderRepositoy : ICafeOrderRepository
{
    private readonly string _connectionString;

    public CafeOrderRepositoy(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<CafeOrder> GetCafeOrdersInTimeframe(DateOnly startDate, DateOnly endDate)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT *
                        FROM CafeOrder o
                        INNER JOIN Server s ON s.ServerID = o.ServerID
                        WHERE OrderDate >= @startDate AND OrderDate <= @endDate;";
            
            var orders = new List<CafeOrder>();
            
            var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            
            cn.Open();

            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var order = new CafeOrder();
                    order.Server = new Server();
                    
                    order.OrderID = (int)dr["OrderID"];
                    order.ServerID = (int)dr["ServerID"];
                    order.PaymentTypeID = (int)dr["PaymentTypeID"];
                    order.OrderDate = (DateTime)dr["OrderDate"];
                    order.SubTotal = (decimal)dr["SubTotal"];
                    order.Tax = (decimal)dr["Tax"];
                    order.Tip = (decimal)dr["Tip"];
                    order.AmountDue = (decimal)dr["AmountDue"];
                    order.Server.ServerID = (int)dr["ServerID"];
                    order.Server.LastName = (string)dr["LastName"];
                    order.Server.FirstName = (string)dr["FirstName"];
                    orders.Add(order);
                }
            }
            return orders;
        }
    }

    public List<CafeOrder> GetCafeOrdersByServer(int serverId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT * FROM CafeOrder
                        WHERE ServerID = @serverId;";
            return cn.Query<CafeOrder>(sql, new { serverId }).ToList();
        }
    }

    public CafeOrder? GetCafeOrder(int orderId)
    {
        var order = new CafeOrder();
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql1 = "SELECT * FROM CafeOrder WHERE OrderID = @orderId;";
            var sql2 = "SELECT * FROM [Server] WHERE ServerID = @serverId;";
            var sql3 = "SELECT * FROM PaymentType WHERE PaymentTypeID = @paymentTypeId;";
            var sql4 = "SELECT * FROM OrderItem WHERE OrderID = @orderId;";
            
            order = cn.QueryFirstOrDefault<CafeOrder>(sql1, new { orderId });
            
            if (order == null) return null;
            
            order.Server = cn.QueryFirst<Server>(sql2, 
                new { serverId = order.ServerID });
            order.PaymentType = cn.QueryFirst<PaymentType>(sql3, 
                new { paymentTypeId = order.PaymentTypeID });
            order.OrderItems = cn.Query<OrderItem>(sql4,
                new { orderId }).ToList();
            
            return order;
        }
    }

    public void AddCafeOrder(CafeOrder cafeOrder)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO CafeOrder (ServerID, PaymentTypeID, OrderDate, SubTotal, Tax, Tip, AmountDue)
                        VALUES (@ServerID, @PaymentTypeID, @OrderDate, @SubTotal, @Tax, @Tip, @AmountDue);
                        SELECT SCOPE_IDENTITY();";
            var p = new
            {
                cafeOrder.ServerID,
                cafeOrder.PaymentTypeID,
                cafeOrder.OrderDate,
                cafeOrder.SubTotal,
                cafeOrder.Tax,
                cafeOrder.Tip,
                cafeOrder.AmountDue
            };
            cafeOrder.OrderID = cn.ExecuteScalar<int>(sql, p);
        }
    }

    public void EditCafeOrder(CafeOrder cafeOrder)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE CafeOrder SET 
                         ServerID = @ServerID,
                         PaymentTypeID = @PaymentTypeID,
                         OrderDate = @OrderDate,
                         SubTotal = @SubTotal,
                         Tax = @Tax,
                         Tip = @Tip,
                         AmountDue = @AmountDue
                     WHERE OrderID = @OrderID;";

            var p = new
            {
                cafeOrder.ServerID,
                cafeOrder.PaymentTypeID,
                cafeOrder.OrderDate,
                cafeOrder.SubTotal,
                cafeOrder.Tax,
                cafeOrder.Tip,
                cafeOrder.AmountDue,
                cafeOrder.OrderID
            };
            cn.Execute(sql, p);
        }
    }

    public void DeleteCafeOrder(int orderId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql1 = "DELETE FROM CafeOrder WHERE OrderID = @orderId;";
            var sql2 = "DELETE FROM OrderItem WHERE OrderID = @orderId;";
            
            var p = new { orderId };
            
            cn.Open();
            using (var transaction = cn.BeginTransaction())
            {
                transaction.Execute(sql2, p);
                transaction.Execute(sql1, p);
                transaction.Commit();
            }
        }
    }
}