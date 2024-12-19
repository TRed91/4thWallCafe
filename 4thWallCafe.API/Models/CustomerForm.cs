using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.API.Models;

public class CustomerForm
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
    public string? State { get; set; }
    [Required]
    [DataType(DataType.PostalCode)]
    public string ZipCode { get; set; }

    public Customer ToEntity()
    {
        return new Customer
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
            Password = Password,
            Street = Street,
            City = City,
            Country = Country,
            State = State,
            ZipCode = ZipCode
        };
    }
}