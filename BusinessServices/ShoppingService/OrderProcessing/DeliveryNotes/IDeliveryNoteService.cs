using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IDeliveryNoteService : IDisposable
    {
        DeliveryNote DeliverOrderItems(int orderHeaderID);
        List<DeliveryNote> GetDeliveryNotesForOrder(int orderID);
        DeliveryNote GetDeliveryNoteByID(int deliveryNoteID);
        Order GetOrder(int orderHeaderID);
    }
}