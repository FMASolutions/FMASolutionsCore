using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemsViewModel
    {
        public AmendOrderItemsViewModel()
        {
            Details = new List<DTOOrderItemDetailed>();
            AvailableItems = new List<ItemViewModel>();
            
        }
        public List<DTOOrderItemDetailed> Details;
        public List<ItemViewModel> AvailableItems {get; set;}
        public StockHierarchyViewModel StockHierarchy {get;set;}
        public string StatusMessage {get; set;}
    }
}