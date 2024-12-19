using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Models;

public class CustomerModel
{
    public int CustomerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
    public string ZipCode { get; set; }

    public CustomerModel() { }

    public CustomerModel(Customer entity)
    {
        CustomerID = entity.CustomerID;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        Email = entity.Email;
        Phone = entity.Phone;
        Street = entity.Street;
        City = entity.City;
        Country = entity.Country;
        State = entity.State;
        ZipCode = entity.ZipCode;
    }
}