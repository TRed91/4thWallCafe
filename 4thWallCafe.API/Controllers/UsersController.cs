using _4thWallCafe.API.Authentication;
using _4thWallCafe.API.Models;
using _4thWallCafe.API.Utilities;
using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace _4thWallCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IJwtService _jwtService;

    public UsersController(ICustomerService customerService, ILogger<UsersController> logger, IConfiguration config, IJwtService jwtService)
    {
        _customerService = customerService;
        _logger = logger;
        _config = config;
        _jwtService = jwtService;
    }
    
    /// <summary>
    /// Returns a single customer
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(CustomerModel),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public ActionResult<CustomerModel> GetCustomer(string email)
    {
        var result = _customerService.GetCustomerByEmail(email);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            return NotFound();
        }

        if (!_jwtService.ValidatePayload(result.Data.CustomerID, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("~/api/signup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult CreateCustomer(CustomerForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        string hashedPassword = PasswordEncryption
            .HashPassword(form.Password, _config["Secret"]);
        
        Console.WriteLine(hashedPassword);
        var customer = form.ToEntity();
        customer.Password = hashedPassword;
        
        var result = _customerService.AddCustomer(customer);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            return StatusCode(500, result.Message);
        }
        
        _logger.LogInformation($"Created new customer. Email: {customer.Email}, ID: {customer.CustomerID}");
        return Created();
    }

    /// <summary>
    /// Updates a single customer
    /// </summary>
    /// <param name="id"></param>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public ActionResult UpdateCustomer(int id, CustomerForm form)
    {
        if (!_jwtService.ValidatePayload(id, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var customerResult = _customerService.GetCustomerById(id);
        if (!customerResult.Ok)
        {
            _logger.LogError(customerResult.Message);
            return NotFound();
        }
        var customer = customerResult.Data;
        customer.FirstName = form.FirstName;
        customer.LastName = form.LastName;
        customer.Email = form.Email;
        customer.Phone = form.Phone;
        customer.Street = form.Street;
        customer.City = form.City;
        customer.State = form.State;
        customer.ZipCode = form.ZipCode;
        customer.Country = form.Country;
        customer.Password = PasswordEncryption
            .HashPassword(form.Password, _config["Secret"]);
        
        var result = _customerService.UpdateCustomer(customer);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            return StatusCode(500);
        }
        
        _logger.LogInformation($"Customer updated. ID: {id}");
        return NoContent();
    }

    /// <summary>
    /// Removes a customer from the db
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public ActionResult DeleteCustomer(int id)
    {
        if (!_jwtService.ValidatePayload(id, HttpContext))
        {
            _logger.LogWarning("Invalid token.");
            return Unauthorized();
        }
        var result = _customerService.DeleteCustomer(id);
        if (!result.Ok)
        {
            _logger.LogError(result.Message);
            return StatusCode(500, result.Message);
        }
        
        _logger.LogInformation($"Customer has been deleted. ID: {id}");
        return NoContent();
    }
}