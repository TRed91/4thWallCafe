﻿using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;
using _4thWallCafe.Core.Models;

namespace _4thWallCafe.Tests.MockRepos;

public class MockItemRepo : IItemRepository
{
    private readonly List<Item> _items;
    private readonly List<Category> _categories;
    private readonly List<ItemPrice> _itemPrices;
    private int itemId;
    private int itemPriceId;

    public MockItemRepo()
    {
        _items = new List<Item>
        {
            new Item { ItemID = 1, ItemName = "Garlic Bread", ItemDescription = "So garlicy", CategoryID = 1 },
            new Item { ItemID = 2, ItemName = "Garlic Sandwich", ItemDescription = "So garlicy", CategoryID = 2 },
            new Item { ItemID = 3, ItemName = "Garlic Beer", ItemDescription = "So garlicy, so beery", CategoryID = 3 },
            new Item { ItemID = 4, ItemName = "Sandwich Bread", ItemDescription = "Just the bread", CategoryID = 1 },
            new Item { ItemID = 5, ItemName = "Desperados", ItemDescription = "Beer mixed with Tequila", CategoryID = 3 },
        };

        _categories = new List<Category>
        {
            new Category { CategoryID = 1, CategoryName = "Bread" },
            new Category { CategoryID = 2, CategoryName = "Sandwich" },
            new Category { CategoryID = 3, CategoryName = "Beer" },
        };

        _itemPrices = new List<ItemPrice>
        {
            new ItemPrice
            {
                ItemPriceID = 1, ItemID = 1, TimeOfDayID = 1, StartDate = DateTime.Today, EndDate = DateTime.Today,
                Price = 10.00m
            },
            new ItemPrice
            {
                ItemPriceID = 2, ItemID = 2, TimeOfDayID = 2, StartDate = DateTime.Today, EndDate = DateTime.Today,
                Price = 15.00m
            },
            new ItemPrice
            {
                ItemPriceID = 3, ItemID = 3, TimeOfDayID = 3, StartDate = DateTime.Today, EndDate = DateTime.Today,
                Price = 20.00m
            },
            new ItemPrice
            {
                ItemPriceID = 4, ItemID = 4, TimeOfDayID = 4, StartDate = DateTime.Today, EndDate = DateTime.Today,
                Price = 5.00m
            },
            new ItemPrice
            {
                ItemPriceID = 5, ItemID = 5, TimeOfDayID = 5, StartDate = DateTime.Today, EndDate = DateTime.Today,
                Price = 3.00m
            },
        };
        
        itemId = _items.Count + 1;
        itemPriceId = _itemPrices.Count + 1;
    }
    
    public List<Item> GetAllItems()
    {
        return _items;
    }

    public List<Item> GetItemsByCategory(int categoryId)
    {
        return _items.Where(i => i.CategoryID == categoryId).ToList();
    }

    public List<ItemPrice> GetItemsByTimeOfDay(int timeOfDayId)
    {
        throw new NotImplementedException();
    }

    public List<ItemPrice> GetItemsByCategoryAndTimeOfDay(int categoryId, int timeOfDayId)
    {
        throw new NotImplementedException();
    }

    public Item? GetItemById(int id)
    {
        return _items.FirstOrDefault(i => i.ItemID == id);
    }

    public ItemPrice? GetItemPriceById(int id)
    {
        return _itemPrices.FirstOrDefault(i => i.ItemPriceID == id);
    }

    public List<Category> GetCategories()
    {
        return _categories;
    }

    public List<TimeOfDay> GetTimeOfDays()
    {
        throw new NotImplementedException();
    }

    public List<ItemReport> GetItemReports(DateOnly startDate, DateOnly endDate)
    {
        throw new NotImplementedException();
    }

    public void AddItem(Item item)
    {
        item.ItemID = itemId++;
        _items.Add(item);
    }

    public void AddItemPrice(ItemPrice itemPrice)
    {
        itemPrice.ItemPriceID = itemPriceId++;
        _itemPrices.Add(itemPrice);
    }

    public void UpdateItem(Item item)
    {
        int index = _items.FindIndex(i => i.ItemID == item.ItemID);
        _items[index] = item;
        
    }

    public void UpdateItemPrice(ItemPrice itemPrice)
    {
        int index = _itemPrices.FindIndex(i => i.ItemPriceID == itemPrice.ItemPriceID);
        _itemPrices[index] = itemPrice;
    }

    public void DeleteItem(int itemId)
    {
        _items.RemoveAll(i => i.ItemID == itemId);
    }
}