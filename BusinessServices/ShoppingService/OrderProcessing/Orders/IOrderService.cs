using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderService : IDisposable
    {
        OrderHeaderDTO GetOrderHeader(int orderHeaderID);
        int CreateOrderWithNewAddress(OrderHeaderCreationDTO orderHeader, AddressLocation newAddress, int customerID);
        int CreateOrderWithExistingAddress(OrderHeaderCreationDTO orderHeader,int addressID, int customerID);
        IEnumerable<OrderPreviewDTO> GetAllOrders();   
        IEnumerable<OrderPreviewDTO> GetOrdersByCustomerCode(string customerCode);           
        OrderHeaderDetailedDTO GetOrderHeaderDetailed(int orderHeaderID);
        Dictionary<int, string> GetOrderStatusDictionary();




        int AddItemToOrder(OrderItemCreationDTO item);
        bool RemoveItemFromOrder(int orderItemID);
        IEnumerable<OrderItemDTO> GetOrderItemsForOrder(int orderHeaderID);
        IEnumerable<OrderItemDetailedDTO> GetOrderItemsDetailed(int orderHeaderID);


        
        
        //Wrappers to other services.
        IEnumerable<int> GetDeliveryNotesForOrder(int orderID);
        IEnumerable<int> GetInvoicesForOrder(int orderHeaderID);
        List<StockHierarchyItem> GetStockHierarchy();
        List<Customer> GetAvailableCustomers();
        List<AddressLocation> GetAvailableAddresses();
        List<CityArea>  GetAvailableCityAreas();
    }
}
