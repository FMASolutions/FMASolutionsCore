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
        
        //Wrappers to other services.
        List<DeliveryNote> GetDeliveryNotesForOrder(int orderID);
        List<Invoice> GetInvoicesForOrder(int orderHeaderID);
        List<StockHierarchyItem> GetStockHierarchy();
        List<Customer> GetAvailableCustomers();
        List<AddressLocation> GetAvailableAddresses();
        List<CityArea>  GetAvailableCityAreas();
        List<CustomerAddress> GetAvailableCustomerAddresses();
        Dictionary<int, string> GetOrderStatusDictionary();
        
    }
}
