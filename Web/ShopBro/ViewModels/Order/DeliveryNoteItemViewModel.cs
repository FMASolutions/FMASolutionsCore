using System;
namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DeliveryNoteItemViewModel
    {
        public DeliveryNoteItemViewModel()
        {
            ItemDeliveryDate = new DateTime();
        }
        public int DeliveryNoteItemID {get; set;}
        public int DeliveryNoteID {get; set;}
        public int ItemID {get; set;}
        public int OrderItemID {get; set;}
        public string ItemDescription {get; set;}
        public DateTime ItemDeliveryDate {get; set;}
        public int ItemQty {get; set;}
        public string StatusMessage {get; set;}
    }
}