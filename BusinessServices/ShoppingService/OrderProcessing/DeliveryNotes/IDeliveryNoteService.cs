using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IDeliveryNoteService : IDisposable
    {
        int DeliverOrderItems(int orderHeaderID);
        IEnumerable<int> GetDeliveryNotesForOrder(int orderID);

        IEnumerable<DeliveryNoteItemDTO> GetDeliveryNoteByDeliveryNoteID(int deliveryNoteID);
    }
}