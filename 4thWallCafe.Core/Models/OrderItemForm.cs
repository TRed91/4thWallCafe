using System.ComponentModel.DataAnnotations;

namespace _4thWallCafe.Core.Models;

public class OrderItemForm
{
    [Required]
    public int ItemPriceID { get; set; }
    [Required]
    [Range(1, 200)]
    public byte Quantity { get; set; }
}