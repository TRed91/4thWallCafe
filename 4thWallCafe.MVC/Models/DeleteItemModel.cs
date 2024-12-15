using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.MVC.Models;

public class DeleteItemModel
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }

    public DeleteItemModel()
    {
        
    }

    public DeleteItemModel(Item item)
    {
        ItemID = item.ItemID;
        ItemName = item.ItemName;
    }
}