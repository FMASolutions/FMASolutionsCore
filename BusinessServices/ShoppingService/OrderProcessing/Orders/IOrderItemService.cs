using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IOrderItemService : IDisposable
    {
        int AddItemToOrder(OrderItemCreationDTO item);
        bool RemoveItemFromOrder(int orderItemID);
        IEnumerable<OrderItemDTO> GetOrderItemsForOrder(int orderHeaderID);
        IEnumerable<OrderItemDetailedDTO> GetOrderItemsDetailed(int orderHeaderID);
    }
}