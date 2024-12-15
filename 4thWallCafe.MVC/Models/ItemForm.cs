using System.ComponentModel.DataAnnotations;

namespace _4thWallCafe.MVC.Models;

public class ItemForm
{
    [Required]
    [Display(Name = "Item Name")]
    public string ItemName { get; set; }
    
    [Required]
    [Display(Name ="Description")]
    public string ItemDescription { get; set; }
    
    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    
    [Display(Name = "Breakfast")]
    [DataType(DataType.Currency)]
    public decimal? BreakfastPrice { get; set; }
    
    [Display(Name = "Lunch")]
    [DataType(DataType.Currency)]
    public decimal? LunchPrice { get; set; }
    
    [Display(Name = "Happy Hour")]
    [DataType(DataType.Currency)]
    public decimal? HappyHourPrice { get; set; }
    
    [Display(Name = "Dinner")]
    [DataType(DataType.Currency)]
    public decimal? DinnerPrice { get; set; }
    
    [Display(Name = "Seasonal")]
    [DataType(DataType.Currency)]
    public decimal? SeasonalPrice { get; set; }
}