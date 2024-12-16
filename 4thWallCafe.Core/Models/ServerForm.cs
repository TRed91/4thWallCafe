using System.ComponentModel.DataAnnotations;

namespace _4thWallCafe.Core.Models;

public class ServerForm : IValidatableObject
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Required]
    [Display(Name = "Date of birth")]
    [DataType(DataType.Date)]
    public DateTime DoB { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (DateTime.Today.AddYears(-18) < DoB.Date)
        {
            errors.Add(new ValidationResult("Server must be at least 18 years old.", ["DoB"]));
        }

        if (FirstName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("First Name must contain only letters.", ["FirstName"]));
        }
        
        if (LastName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("Last Name must contain only letters.", ["LastName"]));
        }
        
        return errors;
    }
}