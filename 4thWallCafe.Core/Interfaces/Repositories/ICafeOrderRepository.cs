﻿using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;

namespace _4thWallCafe.Core.Interfaces;

public interface ICafeOrderRepository
{
    List<CafeOrder> GetCafeOrdersInTimeframe(DateOnly startDate, DateOnly endDate);
    List<CafeOrder> GetCafeOrdersByServer(int serverId);
    CafeOrder? GetCafeOrder(int orderId);
    void AddCafeOrder(CafeOrder cafeOrder);
    void EditCafeOrder(CafeOrder cafeOrder);
    void DeleteCafeOrder(int orderId);
}