namespace _4thWallCafe.Core.Entities;

public class Server
{
    public int ServerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? TermDate { get; set; }
    public DateTime DoB { get; set; }
    
    public List<CafeOrder> Orders { get; set; }
}