using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IOrderHeaderRepo : IDataRepository<OrderHeaderEntity>
    {        
         
         IEnumerable<OrderPreviewDTO> GetAllOrderPreviews();
         IEnumerable<OrderPreviewDTO> GetOrdersByCustomerCode(string customerCode);
         OrderHeaderDetailedDTO GetOrderHeaderDetailed(int orderHeaderID);
         OrderHeaderDTO GetOrderHeader(int orderHeaderID);
         OrderHeaderEntity GetLatestOrder();
         
         
    }
}
