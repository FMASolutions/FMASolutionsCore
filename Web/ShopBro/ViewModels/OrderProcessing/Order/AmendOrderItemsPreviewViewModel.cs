using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemsPreviewViewModel
    {
        public AmendOrderItemsPreviewViewModel()
        {
            HeaderDetail = new OrderPreviewDTO();
            ItemDetails = new List<OrderItemPreviewDTO>();
        }
        public OrderPreviewDTO HeaderDetail {get;set;}
        public List<OrderItemPreviewDTO> ItemDetails {get;set;} 
    }
}