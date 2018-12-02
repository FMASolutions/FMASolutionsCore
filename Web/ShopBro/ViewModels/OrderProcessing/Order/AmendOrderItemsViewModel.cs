using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemsViewModel
    {
        public AmendOrderItemsViewModel()
        {
            ItemDetails = new List<OrderItemDetailedDTO>();
            AvailableItems = new List<ItemViewModel>();
            HeaderDetail = new OrderHeaderDetailedDTO();
            StockHierarchy = new StockHierarchyViewModel();
            
        }
        public List<OrderItemDetailedDTO> ItemDetails;
        public OrderHeaderDetailedDTO HeaderDetail {get;set;}
        public List<ItemViewModel> AvailableItems {get; set;}
        public StockHierarchyViewModel StockHierarchy {get;set;}
        public string StatusMessage {get; set;}
    }
}