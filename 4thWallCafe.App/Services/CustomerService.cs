using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICafeOrderRepository _cafeOrderRepository;

    public CustomerService(ICustomerRepository customerRepository, ICafeOrderRepository cafeOrderRepository)
    {
        _customerRepository = customerRepository;
        _cafeOrderRepository = cafeOrderRepository;
    }
    
    /// <summary>
    /// Returns full customer information including password! Do NOT expose to endpoints.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public Result<Customer> GetCustomerById(int customerId)
    {
        try
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                return ResultFactory.Fail<Customer>("Customer not found");
            }

            return ResultFactory.Success(customer);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Customer>(ex.Message);
        }
    }

    public Result<CustomerModel> GetCustomerByEmail(string email)
    {
        try
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            if (customer == null)
            {
                return ResultFactory.Fail<CustomerModel>("Customer not found");
            }

            return ResultFactory.Success(customer);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<CustomerModel>(ex.Message);
        }
    }

    public Result<List<CustomerModel>> GetCustomers()
    {
        try
        {
            var customers = _customerRepository.GetCustomers();
            return ResultFactory.Success(customers);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CustomerModel>>(ex.Message);
        }
    }

    public Result AddCustomer(Customer customer)
    {
        try
        {
            var existingCustomer = _customerRepository.GetCustomerByEmail(customer.Email);
            if (existingCustomer != null)
            {
                return ResultFactory.Fail("Email already in use");
            }
            _customerRepository.AddCustomer(customer);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result UpdateCustomer(Customer customer)
    {
        try
        {
            _customerRepository.UpdateCustomer(customer);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteCustomer(int customerId)
    {
        try
        {
            _customerRepository.DeleteCustomer(customerId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result CreateOrder(CustomerOrder order)
    {
        try
        {
            _customerRepository.AddCustomerOrder(order);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result<CustomerOrderModel> GetOrderById(int customerOrderId)
    {
        try
        {
            var customerOrder = _customerRepository.GetCustomerOrderById(customerOrderId);
            if (customerOrder == null)
            {
                return ResultFactory.Fail<CustomerOrderModel>("Order not found");
            }

            var customer = _customerRepository.GetCustomerById(customerOrder.CustomerID);
            var cafeOrder = _cafeOrderRepository.GetCafeOrder(customerOrder.OrderID);
            customerOrder.Customer = customer;
            customerOrder.Order = cafeOrder;
            
            var orderModel = new CustomerOrderModel
            {
                CustomerOrderID = customerOrder.CustomerOrderID,
                OrderID = customerOrder.OrderID,
                CustomerID = customerOrder.CustomerID,
                Order = customerOrder.Order,
                Customer = new CustomerModel(customerOrder.Customer),
            };
            
            return ResultFactory.Success(orderModel);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<CustomerOrderModel>(ex.Message);
        }
    }

    public Result UpdateCustomerOrder(CustomerOrder customerOrder)
    {
        try
        {
            _customerRepository.UpdateCustomerOrder(customerOrder);
            _cafeOrderRepository.EditCafeOrder(customerOrder.Order);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteCustomerOrder(int customerOrderId)
    {
        try
        {
            var order = _customerRepository.GetCustomerOrderById(customerOrderId);
            if (order == null)
            {
                return ResultFactory.Fail("Order not found");
            }

            _cafeOrderRepository.DeleteCafeOrder(order.OrderID);
            _customerRepository.DeleteCustomerOrder(order.CustomerOrderID);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}