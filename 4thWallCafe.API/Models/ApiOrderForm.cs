using System.ComponentModel.DataAnnotations;
using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.API.Models;

public class ApiOrderForm
{
    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; }
    [Required]
    public int ServerID { get; set; }
    [Required]
    public int PaymentTypeID { get; set; }
    [Required]
    public decimal Tip { get; set; }

    /// <summary>
    /// Converts ApiOrderForm to CafeOrder, SubTotal and Tax default to 0
    /// </summary>
    /// <returns></returns>
    public CafeOrder ToCafeOrder()
    {
        return new CafeOrder
        {
            ServerID = ServerID,
            PaymentTypeID = PaymentTypeID,
            OrderDate = DateTime.Now,
            SubTotal = 0.00m,
            Tax = 0.00m,
            Tip = Tip,
            AmountDue = Tip
        };
    }
}