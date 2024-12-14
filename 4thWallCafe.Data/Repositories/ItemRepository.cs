using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Models;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;

namespace _4thWallCafe.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly string _connectionString;

    public ItemRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public List<Item> GetAllItems()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT * 
                        FROM Item i INNER JOIN Category c ON i.CategoryID = c.CategoryID
                        ORDER BY CategoryID";
            
            var cmd = new SqlCommand(sql, cn);
            cn.Open();
            using (var dr = cmd.ExecuteReader())
            {
                var items = new List<Item>();
                while (dr.Read())
                {
                    var item = new Item();
                    item.ItemID = (int)dr["ItemID"];
                    item.CategoryID = (int)dr["CategoryID"];
                    item.ItemDescription = (string)dr["ItemDescription"];
                    item.ItemName = (string)dr["ItemName"];
                    item.Category = new Category();
                    item.Category.CategoryID = (int)dr["CategoryID"];
                    item.Category.CategoryName = (string)dr["CategoryName"];
                    items.Add(item);
                }
                return items;
            }
        }
    }

    public List<Item> GetItemsByCategory(int categoryId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = "SELECT * FROM Item WHERE CategoryID = @categoryId";
            
            return cn.Query<Item>(sql, new { categoryId }).ToList();
        }
    }

    public List<ItemPrice> GetItemsByTimeOfDay(int timeOfDayId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var items = new List<ItemPrice>();
            
            var sql = @"SELECT * FROM ItemPrice ip
                        INNER JOIN Item i ON i.ItemID = ip.ItemID
                        INNER JOIN Category c ON c.CategoryID = i.CategoryID
                        WHERE TimeOfDayID = @timeOfDayId AND EndDate IS NULL
                        ORDER BY CategoryName, ItemName";

            var cmd = new SqlCommand(sql, cn);
            cn.Open();
            
            cmd.Parameters.AddWithValue("@timeOfDayId", timeOfDayId);
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var item = new ItemPrice();
                    item.ItemID = (int)dr["ItemID"];
                    item.Price = (decimal)dr["Price"];
                    item.StartDate = DateOnly.FromDateTime((DateTime)dr["StartDate"]);
                    item.TimeOfDayID = (int)dr["TimeOfDayID"];
                    item.Item = new Item();
                    item.Item.CategoryID = (int)dr["CategoryID"];
                    item.Item.ItemName = (string)dr["ItemName"];
                    item.Item.ItemDescription = (string)dr["ItemDescription"];
                    item.Item.Category = new Category();
                    item.Item.Category.CategoryID = (int)dr["CategoryID"];
                    item.Item.Category.CategoryName = (string)dr["CategoryName"];
                    items.Add(item);
                }
            }
            return items;
        }
    }

    public List<ItemPrice> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var items = new List<ItemPrice>();
            
            var sql = @"SELECT * FROM ItemPrice ip
                        INNER JOIN Item i ON i.ItemID = ip.ItemID
                        INNER JOIN Category c ON c.CategoryID = i.CategoryID
                        WHERE TimeOfDayID = @timeOfDayId 
                          AND c.CategoryID = @categoryId
                          AND EndDate IS NULL";
            
            cn.Open();
            
            var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@timeOfDayId", timeOfDayId);
            cmd.Parameters.AddWithValue("@categoryId", categoryId);
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var item = new ItemPrice();
                    item.ItemID = (int)dr["ItemID"];
                    item.Price = (decimal)dr["Price"];
                    item.StartDate = DateOnly.FromDateTime((DateTime)dr["StartDate"]);
                    item.TimeOfDayID = (int)dr["TimeOfDayID"];
                    item.Item = new Item();
                    item.Item.CategoryID = (int)dr["CategoryID"];
                    item.Item.ItemName = (string)dr["ItemName"];
                    item.Item.ItemDescription = (string)dr["ItemDescription"];
                    item.Item.Category = new Category();
                    item.Item.Category.CategoryID = (int)dr["CategoryID"];
                    item.Item.Category.CategoryName = (string)dr["CategoryName"];
                    items.Add(item);
                }
            }
            return items;
        }
    }

    public Item? GetItemById(int id)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT * FROM Item WHERE ItemID = @id;
                        SELECT * FROM ItemPrice WHERE ItemID = @id;";

            using (var multi = cn.QueryMultiple(sql, new { id }))
            {
                var item = multi.ReadFirst<Item>();
                item.ItemPrices = multi.Read<ItemPrice>().AsList();
                
                return item;
            }
        }
    }

    public ItemPrice? GetItemPriceById(int id)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            return cn.QueryFirstOrDefault<ItemPrice>(
                "SELECT * FROM ItemPrice WHERE ItemPriceID = @id;", 
                new { id });
        }
    }

    public List<Category> GetCategories()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            return cn.Query<Category>("SELECT * FROM Category;").ToList();
        }
    }

    public List<TimeOfDay> GetTimeOfDays()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            return cn.Query<TimeOfDay>("SELECT * FROM TimeOfDay;").ToList();
        }
    }

    public List<ItemReport> GetItemReports(DateOnly startDate, DateOnly endDate)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT SUM(ExtendedPrice) AS Revenue, ItemName, CategoryName 
                FROM OrderItem o
                INNER JOIN ItemPrice ip ON o.ItemPriceID = ip.ItemPriceID
                INNER JOIN Item i ON i.ItemID = ip.ItemID
                INNER JOIN Category c ON c.CategoryID = i.CategoryID
                INNER JOIN CafeOrder co ON o.OrderID = co.OrderID
                WHERE OrderDate BETWEEN @startDate AND @endDate
                GROUP BY ItemName, CategoryName 
                ORDER BY  CategoryName; ";

            var p = new
            {
                startDate = startDate.ToDateTime(TimeOnly.Parse("00:00:00")), 
                endDate = endDate.ToDateTime(TimeOnly.Parse("23:59:59")),
            };
            
            return cn.Query<ItemReport>(sql, p).ToList();
        }
    }

    public void AddItem(Item item)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO Item 
                VALUES (@ItemID, @CategoryID, @ItemName, @ItemDescription)
                SELECT SCOPE_IDENTITY()";
            var p = new
            {
                item.ItemID,
                item.CategoryID,
                item.ItemName,
                item.ItemDescription
            };
            item.ItemID = cn.ExecuteScalar<int>(sql, p);
        }
    }

    public void AddItemPrice(ItemPrice itemPrice)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO ItemPrice 
                        VALUES (@ItemID, @TimeOfDayId, @Price, @StartDate, @EndDate)
                        SELECT SCOPE_IDENTITY()";
            var p = new
            {
                itemPrice.ItemID,
                itemPrice.TimeOfDayID,
                itemPrice.Price,
                itemPrice.StartDate,
                itemPrice.EndDate
            };
            itemPrice.ItemPriceID = cn.ExecuteScalar<int>(sql, p);
        }
    }

    public void UpdateItem(Item item)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE Item SET 
                            ItemName = @ItemName, 
                            ItemDescription = @ItemDescription,
                            CategoryID = @CategoryID
                        WHERE ItemID = @ItemID";
            var p = new
            {
                item.ItemID,
                item.ItemName,
                item.ItemDescription,
                item.CategoryID,
            };
            cn.Execute(sql, p);
        }
    }
    
    public void UpdateItemPrice(ItemPrice itemPrice)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE ItemPrice SET 
                            ItemID = @ItemID,
                            TimeOfDayID = @TimeOfDayID,
                            Price = @Price,
                            StartDate = @StartDate,
                            EndDate = @EndDate
                        WHERE ItemPriceID = @ItemPriceID";
            var p = new
            {
                itemPrice.ItemID,
                itemPrice.TimeOfDayID,
                itemPrice.Price,
                itemPrice.StartDate,
                itemPrice.EndDate,
                itemPrice.ItemPriceID
            };
            cn.Execute(sql, p);
        }
    }

    public void DeleteItem(int itemId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql1 = @"DELETE FROM Item WHERE ItemID = @itemId";
            var sql2 = @"DELETE FROM ItemPrice WHERE ItemID = @itemId";
            var p = new { itemId };
            
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