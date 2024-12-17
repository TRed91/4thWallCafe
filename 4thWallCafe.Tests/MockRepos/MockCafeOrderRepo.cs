using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Interfaces;

namespace _4thWallCafe.Tests.MockRepos;

public class MockCafeOrderRepo : ICafeOrderRepository
{
    private readonly List<CafeOrder> _cafeOrders;
    private int id;

    public MockCafeOrderRepo()
    {
        _cafeOrders = new List<CafeOrder>
        {
            new CafeOrder
            {
                OrderID = 1,
                ServerID = 1,
                OrderDate = DateTime.Now.AddDays(-7),
                SubTotal = 10.00m,
                Tax = 1.00m,
                Tip = 1.00m,
                AmountDue = 12.00m,
                PaymentTypeID = 1
            },
            new CafeOrder
            {
                OrderID = 2,
                ServerID = 2,
                OrderDate = DateTime.Now.AddDays(-5),
                SubTotal = 20.00m,
                Tax = 2.00m,
                Tip = 1.00m,
                AmountDue = 23.00m,
                PaymentTypeID = 2
            },
            new CafeOrder
            {
                OrderID = 3,
                ServerID = 3,
                OrderDate = DateTime.Now.AddDays(-14),
                SubTotal = 20.00m,
                Tax = 2.00m,
                Tip = 1.00m,
                AmountDue = 23.00m,
                PaymentTypeID = 3
            },
            new CafeOrder
            {
                OrderID = 4,
                ServerID = 4,
                OrderDate = DateTime.Now.AddDays(-3),
                SubTotal = 10.00m,
                Tax = 1.00m,
                Tip = 1.00m,
                AmountDue = 12.00m,
                PaymentTypeID = 1
            },
            new CafeOrder
            {
                OrderID = 5,
                ServerID = 5,
                OrderDate = DateTime.Now.AddDays(-20),
                SubTotal = 10.00m,
                Tax = 1.00m,
                Tip = 1.00m,
                AmountDue = 12.00m,
                PaymentTypeID = 2
            },
        };
        
        id = _cafeOrders.Count;
    }
    public List<CafeOrder> GetCafeOrdersInTimeframe(DateOnly startDate, DateOnly endDate)
    {
        var start = startDate.ToDateTime(TimeOnly.Parse("00:00:00"));
        var end = endDate.ToDateTime(TimeOnly.Parse("00:00:00"));
        return _cafeOrders.Where(o => o.OrderDate >= start && o.OrderDate <= end).ToList();
    }

    public List<CafeOrder> GetCafeOrdersByServer(int serverId)
    {
        return _cafeOrders.Where(o => o.ServerID == serverId).ToList();
    }

    public CafeOrder? GetCafeOrder(int orderId)
    {
        return _cafeOrders.FirstOrDefault(o => o.OrderID == orderId);
    }

    public void AddCafeOrder(CafeOrder cafeOrder)
    {
        cafeOrder.OrderID = id++;
        _cafeOrders.Add(cafeOrder);
    }

    public void EditCafeOrder(CafeOrder cafeOrder)
    {
        int index = _cafeOrders.FindIndex(o => o.OrderID == cafeOrder.OrderID);
        _cafeOrders[index] = cafeOrder;
    }

    public void DeleteCafeOrder(int orderId)
    {
        _cafeOrders.RemoveAll(o => o.OrderID == orderId);
    }
}