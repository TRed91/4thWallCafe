using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.API.Models;

public class ApiOrderForm
{
    public string CustomerEmail { get; set; }
    public int ServerID { get; set; }
    public int PaymentTypeID { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Tip { get; set; }
    public decimal AmountDue { get; set; }

    /// <summary>
    /// Converts ApiOrderForm to CafeOrder
    /// </summary>
    /// <returns></returns>
    public CafeOrder ToCafeOrder()
    {
        return new CafeOrder
        {
            ServerID = ServerID,
            PaymentTypeID = PaymentTypeID,
            OrderDate = DateTime.Now,
            SubTotal = SubTotal,
            Tax = Tax,
            Tip = Tip,
            AmountDue = AmountDue
        };
    }
}