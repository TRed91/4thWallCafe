using _4thWallCafe.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _4thWallCafe.MVC.Models;

public class AddItemModel
{
    public ItemForm Form { get; set; }
    public SelectList? CategoryList { get; set; }
}