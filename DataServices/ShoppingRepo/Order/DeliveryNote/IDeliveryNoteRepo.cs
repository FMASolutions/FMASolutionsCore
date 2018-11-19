
using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IDeliveryNoteRepo : IDataRepository<DeliveryNoteEntity>
    {
        IEnumerable<DeliveryNoteEntity> GetByOrderHeaderID(int OrderHeaderID)
        {
            DeliveryNoteEntity DeliverOrder(int orderHeaderID);
        }
    }
}
