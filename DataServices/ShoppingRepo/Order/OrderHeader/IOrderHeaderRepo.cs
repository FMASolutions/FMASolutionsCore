using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderHeaderRepo : IDataRepository<OrderHeaderEntity>
    {        
         IEnumerable<OrderItemEntity> GetAllItemsForOrder(int orderID);
         OrderHeaderEntity GetLatestOrder();         
    }
}
