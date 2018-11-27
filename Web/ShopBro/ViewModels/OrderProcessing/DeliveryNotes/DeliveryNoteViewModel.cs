using System;
using System.Collections.Generic;
namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DeliveryNoteViewModel
    {
        public DeliveryNoteViewModel()
        {
            DeliveryDateTime = new DateTime();
            Items = new List<DeliveryNoteItemViewModel>();
        }
        public int DeliveryNoteID {get; set;}
        public int orderHeaderID {get;set;}
        public DateTime DeliveryDateTime {get; set;}        
        public List<DeliveryNoteItemViewModel> Items {get; set;}
        public string StatusMessage {get; set;}
    }
}