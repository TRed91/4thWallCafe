using _4thWallCafe.API.Authentication;
using _4thWallCafe.API.Models;
using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICafeOrderService _cafeOrderService;
    private readonly ILogger _logger;
    private readonly IJwtService _jwtService;

    public OrdersController(
        ICustomerService customerService, 
        ICafeOrderService cafeOrderService, 
        ILogger<OrdersController> logger,
        IJwtService jwtService)
    {
        _customerService = customerService;
        _cafeOrderService = cafeOrderService;
        _logger = logger;
        _jwtService = jwtService;
    }
    
    /// <summary>
    /// Creates a new Order
    /// </summary>
    /// <param name="orderForm"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public ActionResult CreateOrder(ApiOrderForm orderForm)
    {
        // Check if customer exists
        var customerResult = _customerService.GetCustomerByEmail(orderForm.CustomerEmail);
        if (!customerResult.Ok)
        {
            _logger.LogWarning($"Failed to fetch customer: {customerResult.Message}");
            return BadRequest("Invalid customer email");
        }
        
        // Validate token
        if (!_jwtService.ValidatePayload(customerResult.Data.CustomerID, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public ActionResult<CustomerOrderModel> GetOrder(int orderID)
    {
        // fetch order
        var result = _customerService.GetOrderById(orderID);
        if (!result.Ok)
        {
            _logger.LogError($"Failed to fetch order: {result.Message}");
            return NotFound("Order not found");
        }
        
        // validate token
        if (!_jwtService.ValidatePayload(result.Data.CustomerID, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Updates a Customer Order
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
            return NotFound("Order not found");
        }
        
        // Validate token
        if (!_jwtService.ValidatePayload(customerOrderResult.Data.CustomerID, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
        }
        
        // See if a customer with the provided email exists
        var customerResult = _customerService.GetCustomerByEmail(orderForm.CustomerEmail);
        if (!customerResult.Ok)
        {
            _logger.LogWarning($"Failed to fetch customer: {customerResult.Message}");
            return NotFound("Invalid customer email");
        }
        
        // Update the CafeOrder Record
        var updatedOrder = customerOrderResult.Data.Order;
        updatedOrder.SubTotal = orderForm.SubTotal;
        updatedOrder.Tax = orderForm.Tax;
        updatedOrder.Tip = orderForm.Tip;
        updatedOrder.AmountDue = orderForm.AmountDue;
        updatedOrder.PaymentTypeID = orderForm.PaymentTypeID;
        updatedOrder.ServerID = orderForm.ServerID;

        // Update the CustomerOrder Record
        var updatedCustomerOrder = new CustomerOrder
        {
            CustomerOrderID = customerOrderResult.Data.CustomerOrderID,
            CustomerID = customerResult.Data.CustomerID,
            OrderID = updatedOrder.OrderID,
            Order = updatedOrder,
        };
        var result = _customerService.UpdateCustomerOrder(updatedCustomerOrder);
        if (!result.Ok)
        {
            _logger.LogError($"Failed to update customer order: {result.Message}");
            return StatusCode(500, "Internal server error");
        }
        
        _logger.LogInformation($"Successfully updated order with ID: {updatedCustomerOrder.OrderID}");
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
        
        // Validate token
        if (!_jwtService.ValidatePayload(order.Data.CustomerID, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
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