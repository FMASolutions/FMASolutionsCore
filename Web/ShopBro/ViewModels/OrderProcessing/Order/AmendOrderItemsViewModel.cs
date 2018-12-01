using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemsViewModel
    {
        public AmendOrderItemsViewModel()
        {
            ItemDetails = new List<DTOOrderItemDetailed>();
            AvailableItems = new List<ItemViewModel>();
            HeaderDetail = new DTOOrderHeaderDetailed();
            StockHierarchy = new StockHierarchyViewModel();
            
        }
        public List<DTOOrderItemDetailed> ItemDetails;
        public DTOOrderHeaderDetailed HeaderDetail {get;set;}
        public List<ItemViewModel> AvailableItems {get; set;}
        public StockHierarchyViewModel StockHierarchy {get;set;}
        public string StatusMessage {get; set;}
    }
}