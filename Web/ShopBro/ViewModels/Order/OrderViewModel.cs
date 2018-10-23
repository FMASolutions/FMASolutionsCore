using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            AvailableCustomers = new Dictionary<int, string>();
            AvailableItems = new Dictionary<int, string>();
        }

        public int OrderID {get; set;}
        public string OrderStatus {get;set;}

        public string StatusMessage {get; set;}
        public List<OrderItemViewModel> ExistingItems {get; set;}
        public int NewItemID {get;set;}
        public Dictionary<int,string> AvailableItems {get; set;}
        public Dictionary<int,string> AvailableCustomers {get; set;}
        public int CustomerID {get; set;}
    }
}