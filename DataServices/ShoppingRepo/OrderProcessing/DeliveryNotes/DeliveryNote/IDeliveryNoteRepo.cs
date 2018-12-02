
using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IDeliveryNoteRepo : IDataRepository<DeliveryNoteEntity>
    {
        IEnumerable<int> GetByOrderHeaderID(int OrderHeaderID);
        int DeliverOrder(int orderHeaderID);
        
        IEnumerable<DeliveryNoteItemDTO> GetDeliveryNoteItems(int deliveryNoteID);
    }
}
