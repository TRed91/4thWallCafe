using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Models;
using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;

namespace _4thWallCafe.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public Customer? GetCustomerById(int customerId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            return cn.QueryFirstOrDefault<Customer>("SELECT * FROM Customer WHERE CustomerID = @customerId",
                new { customerId });
        }
    }

    public CustomerModel? GetCustomerByEmail(string email)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var customer = cn.QueryFirstOrDefault<Customer>("SELECT * FROM Customer WHERE Email = @email",
                new { email });
            
            if (customer == null) return null;
            
            return new CustomerModel(customer);
        }
    }

    public List<CustomerModel> GetCustomers()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            return cn.Query<CustomerModel>("SELECT * FROM Customer").ToList();
        }
    }

    public void AddCustomer(Customer customer)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO Customer (LastName, FirstName, Email, Phone, Password, Street, City, State, ZipCode, Country)
                        VALUES (@Lastname, @FirstName, @Email, @Phone, @Password, @Street, @City, @State, @ZipCode, @Country);

                        SELECT SCOPE_IDENTITY();";
            var p = new
            {
                customer.LastName,
                customer.FirstName,
                customer.Email,
                customer.Phone,
                customer.Password,
                customer.Street,
                customer.City,
                customer.State,
                customer.ZipCode,
                customer.Country
            };
            cn.ExecuteScalar<int>(sql, p);
        }
    }

    public void UpdateCustomer(Customer customer)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE Customer SET 
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email,
                        Phone = @Phone,
                        Password = @Password,
                        Street = @Street,
                        City = @City,
                        State = @State,
                        ZipCode = @ZipCode,
                        Country = @Country
                    WHERE CustomerID = @CustomerID;";

            var p = new
            {
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Phone,
                customer.Password,
                customer.Street,
                customer.City,
                customer.State,
                customer.ZipCode,
                customer.Country,
                customer.CustomerID
            };
            
            cn.Execute(sql, p);
        }
    }

    public void DeleteCustomer(int customerId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql1 = @"DELETE FROM Customer WHERE CustomerID = @customerId";
            var sql2 = @"DELETE FROM CustomerOrder WHERE CustomerID = @customerId";
            var p = new { customerId };
            
            cn.Open();
            using (var tran = cn.BeginTransaction())
            {
                tran.Execute(sql2, p);
                tran.Execute(sql1, p);
                tran.Commit();
            }
        }
    }

    public void AddCustomerOrder(CustomerOrder customerOrder)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO CustomerOrder (CustomerID, OrderID) 
                        VALUES (@CustomerID, @OrderID);
                        SELECT SCOPE_IDENTITY();";
            var p = new { customerOrder.CustomerID, customerOrder.OrderID };
            
            customerOrder.CustomerOrderID = cn.ExecuteScalar<int>(sql, p);
        }
    }

    public CustomerOrder? GetCustomerOrderById(int customerOrderId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT * FROM CustomerOrder WHERE CustomerOrderID = @customerOrderId";
            return cn.QueryFirstOrDefault<CustomerOrder>(sql, new { customerOrderId });
        }
    }

    public void UpdateCustomerOrder(CustomerOrder customerOrder)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var sql = @"UPDATE CustomerOrder SET 
                         CustomerID = @CustomerID,
                         OrderID = @OrderID
                         WHERE CustomerOrderID = @CustomerOrderID;";
            var p = new
            {
                customerOrder.CustomerID, 
                customerOrder.OrderID, 
                customerOrder.CustomerOrderID
            };
            cn.Execute(sql, p);
        }
    }

    public void DeleteCustomerOrder(int customerOrderId)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            cn.Execute("DELETE FROM CustomerOrder WHERE CustomerOrderID = @customerOrderId", 
                new { customerOrderId });
        }
    }
}