using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Interfaces;

public interface IItemRepository
{
    List<Item> GetAllItems();
    List<Item> GetItemsByCategory(int categoryId);
    List<Item> GetItemsByTimeOfDay(int timeOfDayId);
    List<Item> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId);
    void AddItem(Item item);
    void AddItemPrice(ItemPrice itemPrice);
    void UpdateItem(Item item);
    void UpdateItemPrice(ItemPrice itemPrice);
    void DeleteItem(int itemId);
}