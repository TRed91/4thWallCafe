namespace _4thWallCafe.Core.Entities;

public class Customer
{
    public int CustomerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
    public string ZipCode { get; set; }
    
    public List<CafeOrder> Orders { get; set; }
}