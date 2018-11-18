using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            AvailableCustomers = new Dictionary<int, string>();
            AvailableItems = new List<ItemViewModel>();
            ExistingItems = new List<OrderItemViewModel>();
            StockHierarchy = new StockHierarchyViewModel();
            DistinctItemStatusList = new List<string>();
        }

        public int OrderID {get; set;}
        public string OrderStatus {get;set;}

        public string StatusMessage {get; set;}
        public List<OrderItemViewModel> ExistingItems {get; set;}
        public List<ItemViewModel> AvailableItems {get; set;}
        public StockHierarchyViewModel StockHierarchy {get;set;}
        public Dictionary<int,string> AvailableCustomers {get; set;}
        public List<string> DistinctItemStatusList {get; set;}
        public int CustomerID {get; set;}
        public int SelectedItem {get;set;}
    }
}