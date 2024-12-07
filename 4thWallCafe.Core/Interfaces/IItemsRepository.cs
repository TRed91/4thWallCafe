using _4thWallCafe.Core.Entities;

namespace _4thWallCafe.Core.Interfaces;

public interface IItemsRepository
{
    List<Item> GetAllItems();
    List<Item> GetItemsByCategory(int categoryId);
    List<Item> GetItemsByTimeOfDay(int timeOfDayId);
    List<Item> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId);
    void AddItem(Item item);
    void UpdateItem(Item item);
    void DeleteItem(Item item);
}