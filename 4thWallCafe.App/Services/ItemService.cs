using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Interfaces.Services;
using _4thWallCafe.Core.Models;
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

    public Result<List<MenuItem>> GetItemByTimeOfDay(int timeOfDayId)
    {
        try
        {
            var items = _itemRepository.GetItemsByTimeOfDay(timeOfDayId);
            var menuItems = new List<MenuItem>();
            foreach (var item in items)
            {
                menuItems.Add(new MenuItem(item));
            }
            return ResultFactory.Success(menuItems);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<MenuItem>>(ex.Message);
        }
    }

    public Result<List<MenuItem>> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId)
    {
        try
        {
            var items = _itemRepository.GetItemsByCategoryAndTimeOfDay(
                categoryId, timeOfDayId);
            var menuItems = new List<MenuItem>();
            foreach (var item in items)
            {
                menuItems.Add(new MenuItem(item));
            }
            return ResultFactory.Success(menuItems);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<MenuItem>>(ex.Message);
        }
    }

    public Result<List<Category>> GetCategories()
    {
        try
        {
            var categories = _itemRepository.GetCategories();
            return ResultFactory.Success(categories);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Category>>(ex.Message);
        }
    }

    public Result<List<TimeOfDay>> GetTimeOfDays()
    {
        try
        {
            var timeOfDays = _itemRepository.GetTimeOfDays();
            return ResultFactory.Success(timeOfDays);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TimeOfDay>>(ex.Message);
        }
    }

    public Result<List<ItemReport>> GetItemReports(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var reports = _itemRepository.GetItemReports(startDate, endDate);
            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<ItemReport>>(ex.Message);
        }
    }

    public Result<Item> GetItemById(int id)
    {
        try
        {
            var item = _itemRepository.GetItemById(id);
            if (item == null)
            {
                return ResultFactory.Fail<Item>("Item not found");
            }
            return ResultFactory.Success(item);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Item>(ex.Message);
        }
    }

    public Result AddItem(AddItem addItem)
    {
        var item = new Item
        {
            ItemName = addItem.ItemName,
            ItemDescription = addItem.ItemDescription,
            CategoryID = addItem.CategoryId,
        };
        try
        {
            _itemRepository.AddItem(item);
            var itemPrices = addItem.PriceFields.Select(p => new ItemPrice
            {
                ItemID = item.ItemID,
                Price = p.Price,
                TimeOfDayID = p.TimeOfDayId,
                StartDate = DateTime.Today,
            }).ToList();
            foreach (var price in itemPrices)
            {
                _itemRepository.AddItemPrice(price);
            }
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result UpdateItem(int id, AddItem addItem)
    {
        try
        {
            var item = _itemRepository.GetItemById(id);
            if (item == null)
            {
                return ResultFactory.Fail("Item not found");
            }
            item.ItemName = addItem.ItemName;
            item.ItemDescription = addItem.ItemDescription;
            item.CategoryID = addItem.CategoryId;
            _itemRepository.UpdateItem(item);
            foreach (var price in item.ItemPrices)
            {
                price.Price = addItem.PriceFields
                    .Where(p => p.TimeOfDayId == price.TimeOfDayID)
                    .Select(p => p.Price)
                    .First();
                _itemRepository.UpdateItemPrice(price);
            }
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            _itemRepository.DeleteItem(id);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}