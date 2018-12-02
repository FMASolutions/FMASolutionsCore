using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class DeliveryNoteItemDTO
    {
        public int DeliveryNoteItemID {get;set;}
        public int DeliveryNoteID {get;set;}
        public int OrderHeaderID {get;set;}
        public DateTime DeliveryDate {get;set;}
        public int OrderItemID {get;set;}
        public string OrderItemDescription {get;set;}
        public int OrderItemQty {get;set;}

    }
}