using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;
namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DisplayDeliveryNoteViewModel
    {
        public DisplayDeliveryNoteViewModel()
        {
            DeliveryNoteItems = new List<DeliveryNoteItemDTO>();
        }

        public List<DeliveryNoteItemDTO> DeliveryNoteItems {get;set;}
        public string StatusMessage {get;set;}
    }
}