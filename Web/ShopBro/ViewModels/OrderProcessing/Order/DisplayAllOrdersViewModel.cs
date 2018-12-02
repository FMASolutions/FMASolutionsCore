using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DisplayAllOrdersViewModel
    {
        public DisplayAllOrdersViewModel()
        {
            Orders = new List<OrderPreviewDTO>();
        }
        public List<OrderPreviewDTO> Orders {get;set;}
        public string StatusMessage {get;set;}       
    }
}