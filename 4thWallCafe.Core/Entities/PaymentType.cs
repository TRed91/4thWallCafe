namespace _4thWallCafe.Core.Entities;

public class PaymentType
{
    public int PaymentTypeID { get; set; }
    public string PaymentTypeName { get; set; }
    
    public List <CafeOrder> Orders { get; set; }
}