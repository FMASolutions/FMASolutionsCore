using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderService : IDisposable
    {
        Order GetByID(int id);        
        int CreateOrder(OrderHeader model,AddressLocation newAddress = null);
        int AddItemToOrder(OrderItem item);
        bool RemoveItemFromOrder(OrderItem item);
        
        List<Order> GetAllOrders();
        bool UpdateHeader(Order newModel);
        DeliveryNote DeliverOrderItems(int orderHeaderID);
        List<DeliveryNote> GetDeliveryNotesForOrder(int orderID);
        List<StockHierarchyItem> GetStockHierarchy();
        List<Customer> GetAvailableCustomers();
        List<AddressLocation> GetAvailableAddresses();
        List<CityArea>  GetAvailableCityAreas();
        Dictionary<int, string> GetOrderStatusDictionary();
    }
}
