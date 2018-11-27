using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderItemRepo : IDataRepository<OrderItemEntity>
    {
        int GetLatestOrderItemByOrder(int orderHeaderID);
    }
}