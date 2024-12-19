using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface ICustomerService
{
    Result<Customer> GetCustomerById(int customerId);
    Result<CustomerModel> GetCustomerByEmail(string email);
    Result<List<CustomerModel>> GetCustomers();
    Result AddCustomer(Customer customer);
    Result UpdateCustomer(Customer customer);
    Result DeleteCustomer(int customerId);
}