using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface IItemService
{
    Result<List<Item>> GetItems();
    Result<List<Item>> GetItemsByCategory(int categoryId);
    Result<List<Item>> GetItemByTimeOfDay(int timeOfDayId);
    Result<List<Item>> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId);
    Result AddItem(Item item);
    Result UpdateItem(Item item);
    Result DeleteItem(Item item);
}