using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderItemService : IDisposable
    {
        int AddItemToOrder(OrderItem item);
        bool RemoveItemFromOrder(OrderItem item);
        List<OrderItem> GetOrderItemsForOrder(int orderHeaderID);
        IEnumerable<DTOOrderItemDetailed> GetOrderItemsDetailed(int orderHeaderID);
    }
}