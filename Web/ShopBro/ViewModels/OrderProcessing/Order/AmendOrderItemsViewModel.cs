using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemsViewModel
    {
        public AmendOrderItemsViewModel()
        {
            Details = new List<AmendOrderItemViewModel>();
            AvailableItems = new List<ItemViewModel>();
        }
        public List<AmendOrderItemViewModel> Details {get; set;}
        public List<ItemViewModel> AvailableItems {get; set;}
        public StockHierarchyViewModel StockHierarchy {get;set;}
        public string StatusMessage {get; set;}
    }
}