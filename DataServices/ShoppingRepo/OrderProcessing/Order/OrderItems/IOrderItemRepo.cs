using FMASolutionsCore.DataServices.DataRepository;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderItemRepo : IDataRepository<OrderItemEntity>
    {
        int GetLatestOrderItemByOrder(int orderHeaderID);
        IEnumerable<DTOOrderItemDetailed> GetOrderItemsDetailedForOrder(int orderHeaderID);
        IEnumerable<OrderItemEntity> GetOrderItemsForOrder(int orderHeaderID);
    }
}