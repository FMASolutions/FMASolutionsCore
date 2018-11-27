using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class OrdersViewModel
    {
        public OrdersViewModel()
        {
            Orders = new List<OrderViewModel>();
        }
        public List<OrderViewModel> Orders {get; set;}
        public string StatusMessage {get; set;}
    }
}