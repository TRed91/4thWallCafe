using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;

namespace _4thWallCafe.Core.Interfaces;

public interface ICustomerRepository
{
    Customer? GetCustomerById(int customerId);
    CustomerModel? GetCustomerByEmail(string email);
    List<CustomerModel> GetCustomers();
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
    void AddCustomerOrder(CustomerOrder customerOrder);
    CustomerOrder? GetCustomerOrderById(int customerOrderId);
    void UpdateCustomerOrder(CustomerOrder customerOrder);
    void DeleteCustomerOrder(int customerOrderId);
}