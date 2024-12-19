using _4thWallCafe.API.Models;
using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICafeOrderService _cafeOrderService;
    private readonly ILogger _logger;

    public OrdersController(
        ICustomerService customerService, 
        ICafeOrderService cafeOrderService, 
        ILogger<OrdersController> logger)
    {
        _customerService = customerService;
        _cafeOrderService = cafeOrderService;
        _logger = logger;
    }
    
    /// <summary>
    /// Creates a new Order
    /// </summary>
    /// <param name="orderForm"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult CreateOrder(ApiOrderForm orderForm)
    {
        // Check if customer exists
        var customerResult = _customerService.GetCustomerByEmail(orderForm.CustomerEmail);
        if (!customerResult.Ok)
        {
            _logger.LogWarning($"Failed to fetch customer: {customerResult.Message}");
            return BadRequest("Invalid customer email");
        }
        
        // Create a CafeOrder record
        var cafeOrder = orderForm.ToCafeOrder();
        
        var addCafeOrderResult = _cafeOrderService.AddCafeOrder(cafeOrder);
        if (!addCafeOrderResult.Ok)
        {
            _logger.LogError($"Failed to add cafe order: {addCafeOrderResult.Message}");
            return StatusCode(500, "Internal server error");
        }

        // Create a CustomerOrder record using CustomerID and OrderID 
        var customerOrder = new CustomerOrder
        {
            CustomerID = customerResult.Data.CustomerID,
            OrderID = cafeOrder.OrderID,
        };
        var addCustomerOrderResult = _customerService.CreateOrder(customerOrder);
        if (!addCustomerOrderResult.Ok)
        {
            _logger.LogError($"Failed to create api order: {addCustomerOrderResult.Message}");
            return StatusCode(500, "Internal server error");
        }
        
        _logger.LogInformation($"Successfully created order with ID: {customerOrder.CustomerOrderID}");
        return Created();
    }

    /// <summary>
    /// Get a CustomerOrder including Customer and CafeOrder Data
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns></returns>
    [HttpGet("{orderID}")]
    [ProducesResponseType(typeof(CustomerOrderModel),StatusCodes.Status200OK)]
    public ActionResult<CustomerOrderModel> GetOrder(int orderID)
    {
        var result = _customerService.GetOrderById(orderID);
        if (!result.Ok)
        {
            _logger.LogError($"Failed to fetch order: {result.Message}");
            return StatusCode(500, "Internal server error");
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Updated a Customer Order
    /// </summary>
    /// <param name="orderID"></param>
    /// <param name="orderForm"></param>
    /// <returns></returns>
    [HttpPut("{orderID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateOrder(int orderID, ApiOrderForm orderForm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // See if the Customer Order exists
        var customerOrderResult = _customerService.GetOrderById(orderID);
        if (!customerOrderResult.Ok)
        {
            _logger.LogError($"Failed to fetch order: {customerOrderResult.Message}");
            return StatusCode(500, "Internal server error");
        }
        
        // See if a customer with the provided email exists
        var customerResult = _customerService.GetCustomerByEmail(orderForm.CustomerEmail);
        if (!customerResult.Ok)
        {
            _logger.LogWarning($"Failed to fetch customer: {customerResult.Message}");
            return NotFound("Invalid customer email");
        }
        
        // Update the CafeOrder Record
        var updatedCafeOrder = orderForm.ToCafeOrder();
        var cafeOrderUpdateResult = _cafeOrderService.UpdateCafeOrder(updatedCafeOrder);
        if (!cafeOrderUpdateResult.Ok)
        {
            _logger.LogError($"Failed to update cafe order: {cafeOrderUpdateResult.Message}");
            return StatusCode(500, "Internal server error");
        }

        // Update the CustomerOrder Record
        var customerOrder = new CustomerOrder
        {
            CustomerID = customerResult.Data.CustomerID,
            OrderID = updatedCafeOrder.OrderID,
        };
        var result = _customerService.UpdateCustomerOrder(customerOrder);
        if (!result.Ok)
        {
            _logger.LogError($"Failed to update customer order: {result.Message}");
            return StatusCode(500, "Internal server error");
        }
        
        _logger.LogInformation($"Successfully updated order with ID: {customerOrder.OrderID}");
        return NoContent();
    }

    /// <summary>
    /// Delete a Customer Order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns></returns>
    [HttpDelete("{orderID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteOrder(int orderID)
    {
        // Check if order exists
        var order = _customerService.GetOrderById(orderID);
        if (!order.Ok)
        {
            _logger.LogWarning($"Failed to fetch order: {order.Message}");
            return NotFound();
        }
        
        // Delete the order
        var result = _customerService.DeleteCustomerOrder(orderID);
        if (!result.Ok)
        {
            _logger.LogError($"Failed to delete order: {result.Message}");
            return StatusCode(500, "Internal server error");
        }
        _logger.LogInformation($"Successfully deleted order with ID: {orderID}");
        return NoContent();
    }
}