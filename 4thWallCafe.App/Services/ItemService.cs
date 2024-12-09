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

    public Result AddItem(ItemForm itemForm)
    {
        var item = new Item
        {
            ItemName = itemForm.ItemName,
            ItemDescription = itemForm.ItemDescription,
            CategoryID = itemForm.CategoryId,
        };
        try
        {
            _itemRepository.AddItem(item);
            var itemPrices = itemForm.PriceFields.Select(p => new ItemPrice
            {
                ItemID = item.ItemID,
                Price = p.Price,
                TimeOfDayID = p.TimeOfDayId,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
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

    public Result UpdateItem(int id, ItemForm itemForm)
    {
        try
        {
            var item = _itemRepository.GetItemById(id);
            if (item == null)
            {
                return ResultFactory.Fail("Item not found");
            }
            item.ItemName = itemForm.ItemName;
            item.ItemDescription = itemForm.ItemDescription;
            item.CategoryID = itemForm.CategoryId;
            _itemRepository.UpdateItem(item);
            foreach (var price in item.ItemPrices)
            {
                price.Price = itemForm.PriceFields
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