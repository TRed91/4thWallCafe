using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.API.Models;

public class CustomerForm : IValidatableObject
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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (Password.Length < 8)
        {
            errors.Add(new ValidationResult("Password must be at least 8 characters", ["Password"]));
        }
        if (!Password.Any(char.IsDigit))
        {
            errors.Add(new ValidationResult("Password must contain a digit", ["Password"]));
        }
        if (!Password.Any(char.IsLower))
        {
            errors.Add(new ValidationResult("Password must contain a lowercase letter", ["Password"]));
        }
        if (!Password.Any(char.IsUpper))
        {
            errors.Add(new ValidationResult("Password must contain a uppercase letter", ["Password"]));
        }
        if (!Password.Any(char.IsSymbol))
        {
            errors.Add(new ValidationResult("Password must contain a symbol", ["Password"]));
        }

        if (FirstName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("First name must contain only letters", ["FirstName"]));
        }
        if (LastName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("Last name must contain only letters", ["LastName"]));
        }
        
        return errors;
    }
}