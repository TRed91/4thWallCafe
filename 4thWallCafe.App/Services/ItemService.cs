using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Utilities;

namespace _4thWallCafe.App.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Result<List<Item>> GetItems()
    {
        try
        {
            var items = _itemRepository.GetAllItems();
            return ResultFactory.Success(items);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Item>>(ex.Message);
        }
    }

    public Result<List<Item>> GetItemsByCategory(int categoryId)
    {
        try
        {
            var items = _itemRepository.GetItemsByCategory(categoryId);
            return ResultFactory.Success(items);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Item>>(ex.Message);
        }
    }

    public Result<List<Item>> GetItemByTimeOfDay(int timeOfDayId)
    {
        try
        {
            var items = _itemRepository.GetItemsByTimeOfDay(timeOfDayId);
            return ResultFactory.Success(items);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Item>>(ex.Message);
        }
    }

    public Result<List<Item>> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId)
    {
        try
        {
            var items = _itemRepository.GetItemsByCategoryAndTimeOfDay(
                categoryId, timeOfDayId);
            return ResultFactory.Success(items);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Item>>(ex.Message);
        }
    }

    public Result AddItem(Item item)
    {
        throw new NotImplementedException();
    }

    public Result UpdateItem(Item item)
    {
        throw new NotImplementedException();
    }

    public Result DeleteItem(Item item)
    {
        throw new NotImplementedException();
    }
}