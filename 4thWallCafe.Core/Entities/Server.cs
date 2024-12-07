namespace _4thWallCafe.Core.Entities;

public class Server
{
    public int ServerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly HireDate { get; set; }
    public DateOnly TermDate { get; set; }
    public DateOnly DoB { get; set; }
    
    public List<CafeOrder> Orders { get; set; }
}