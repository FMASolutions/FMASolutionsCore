using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderService : IDisposable
    {
        Order GetByID(int id);        
        int CreateHeader(OrderHeader model);
        int AddItemToOrder(OrderItem item);
        bool RemoveItemFromOrder(OrderItem item);
        
        List<Order> GetAllOrders();
        bool UpdateHeader(Order newModel);
        int DeliverExistingItems(int orderHeaderID);
        List<StockHierarchyItem> GetStockHierarchy();
        List<Customer> GetAvailableCustomers();
        Dictionary<int, string> GetOrderStatusDictionary();
    }
}