using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderService : IDisposable
    {
        Order GetByID(int id);
        int CreateOrder(OrderHeader model,AddressLocation newAddress = null);
        List<Order> GetAllOrders();              
        DTOOrderHeaderDetailed GetOrderHeaderDetailed(int orderHeaderID);
        
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
