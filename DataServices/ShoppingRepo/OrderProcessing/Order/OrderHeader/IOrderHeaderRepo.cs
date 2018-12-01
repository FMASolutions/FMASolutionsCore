using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderHeaderRepo : IDataRepository<OrderHeaderEntity>
    {        
         IEnumerable<OrderItemEntity> GetAllItemsForOrder(int orderID);
         DTOOrderHeaderDetailed GetOrderHeaderDetailed(int orderHeaderID);
         OrderHeaderEntity GetLatestOrder();
         
         
    }
}
