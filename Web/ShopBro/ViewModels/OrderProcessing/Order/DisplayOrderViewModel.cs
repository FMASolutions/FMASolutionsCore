using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DisplayOrderViewModel
    {
        public DisplayOrderViewModel()
        {
            OrderHeader = new OrderHeaderDTO();
            OrderItems = new List<OrderItemDTO>();
            DeliveryNotesForOrder = new List<int>();
            InvoicesForOrder = new List<int>();
        }
        public OrderHeaderDTO OrderHeader {get;set;}
        public List<OrderItemDTO> OrderItems {get;set;}
        public List<int> DeliveryNotesForOrder {get;set;}
        public List<int> InvoicesForOrder {get;set;}
        public bool HasItemsAtEstimateStage {get {return OrderItems.Exists(x => x.OrderItemStatus == "Estimate");}}
        public bool HasItemsAtDeliveredStage {get {return OrderItems.Exists(x => x.OrderItemStatus == "Delivered");}}
        public string StatusMessage {get;set;}
    }
}