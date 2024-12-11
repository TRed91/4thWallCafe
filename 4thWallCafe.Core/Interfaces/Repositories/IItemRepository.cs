using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Interfaces;

public interface IItemRepository
{
    List<Item> GetAllItems();
    List<Item> GetItemsByCategory(int categoryId);
    List<ItemPrice> GetItemsByTimeOfDay(int timeOfDayId);
    List<ItemPrice> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId);
    Item? GetItemById(int id);
    
    ItemPrice? GetItemPriceById(int id);
    List<Category> GetCategories();
    List<TimeOfDay> GetTimeOfDays();
    void AddItem(Item item);
    void AddItemPrice(ItemPrice itemPrice);
    void UpdateItem(Item item);
    void UpdateItemPrice(ItemPrice itemPrice);
    void DeleteItem(int itemId);
}