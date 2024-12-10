using System.ComponentModel.DataAnnotations;

namespace _4thWallCafe.MVC.db.IdentityModels;

public class LoginUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}