using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.Core.Interfaces.Services;

public interface IItemService
{
    Result<List<Item>> GetItems();
    Result<List<Item>> GetItemsByCategory(int categoryId);
    Result<List<MenuItem>> GetItemByTimeOfDay(int timeOfDayId);
    Result<List<MenuItem>> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId);
    Result<List<Category>> GetCategories();
    Result<List<TimeOfDay>> GetTimeOfDays();
    Result<List<ItemReport>> GetItemReports(DateOnly startDate, DateOnly endDate);
    Result<Item> GetItemById(int id);
    Result AddItem(AddItem addItem);
    Result UpdateItem(int id, AddItem addItem);
    Result DeleteItem(int id);
}