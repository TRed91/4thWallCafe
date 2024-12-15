using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Models;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;

namespace _4thWallCafe.Data.Repositories;

public class CafeOrderRepository : ICafeOrderRepository
{
    private readonly string _connectionString;

    public CafeOrderRepository(string connectionString)
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
            var sql = @"SELECT * FROM CafeOrder WHERE ServerID = @serverId;";
            return cn.Query<CafeOrder>(sql, new { serverId }).ToList();
        }
    }

    public CafeOrder? GetCafeOrder(int orderId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql1 = "SELECT * FROM CafeOrder WHERE OrderID = @orderId;";
            var sql2 = @"SELECT * FROM [Server] WHERE ServerID = @serverId;
                        SELECT * FROM PaymentType WHERE PaymentTypeID = @paymentTypeId;";

            var sql3 = @"SELECT * FROM OrderItem oi
                        INNER JOIN ItemPrice ip ON ip.ItemPriceID = oi.ItemPriceID
                        INNER JOIN Item i ON i.ItemID = ip.ItemID
                        INNER JOIN Category c ON c.CategoryID = i.CategoryID
                        WHERE OrderID = @orderId;";

            var order = cn.QueryFirstOrDefault<CafeOrder>(sql1, new { orderId });
            if (order == null) return null;
            var p = new
            {
                serverId = order.ServerID,
                paymentTypeId = order.PaymentTypeID
            };
            using (var multi = cn.QueryMultiple(sql2, p))
            {
                order.Server = multi.ReadFirst<Server>();
                order.PaymentType = multi.ReadFirst<PaymentType>();
                order.OrderItems = new List<OrderItem>();
            }
            var cmd = new SqlCommand(sql3, cn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cn.Open();
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var orderItem = new OrderItem();
                    orderItem.OrderItemID = (int)dr["OrderItemID"];
                    orderItem.OrderID = (int)dr["OrderID"];
                    orderItem.ItemPriceID = (int)dr["ItemPriceID"];
                    orderItem.Quantity = (byte)dr["Quantity"];
                    orderItem.ExtendedPrice = (decimal)dr["ExtendedPrice"];
                    orderItem.ItemPrice = new ItemPrice
                    {
                        ItemPriceID = (int)dr["ItemPriceID"],
                        ItemID = (int)dr["ItemID"],
                        TimeOfDayID = (int)dr["TimeOfDayID"],
                        Price = (decimal)dr["Price"],
                        StartDate = (DateTime)dr["StartDate"],
                        EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime)dr["EndDate"],
                        Item = new Item
                        {
                            ItemID = (int)dr["ItemID"],
                            CategoryID = (int)dr["CategoryID"],
                            ItemName = (string)dr["ItemName"],
                            ItemDescription = (string)dr["ItemDescription"],
                            Category = new Category
                            {
                                CategoryID = (int)dr["CategoryID"],
                                CategoryName = (string)dr["CategoryName"],
                            }
                        }
                    };
                    order.OrderItems.Add(orderItem);
                }
            }

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