using FMASolutionsCore.DataServices.DataRepository;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderItemRepo : IDataRepository<OrderItemEntity>
    {
        IEnumerable<OrderItemEntity> GetAllItemsForOrder(int orderID);
        int GetLatestOrderItemByOrder(int orderHeaderID);
        IEnumerable<OrderItemDetailedDTO> GetOrderItemsDetailedForOrder(int orderHeaderID);
        IEnumerable<OrderItemDTO> GetOrderItemsForOrder(int orderHeaderID);
    }
}