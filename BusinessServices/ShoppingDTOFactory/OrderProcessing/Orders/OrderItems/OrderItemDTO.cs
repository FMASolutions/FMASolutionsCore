
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderItemDTO
    {
        public int OrderItemID {get;set;}
        public string OrderItemDescription {get;set;}
        public int OrderItemQty {get;set;}
        public int OrderItemUnitPrice {get;set;}
        public string OrderItemStatus {get;set;}
        public int ItemID {get;set;}
    }
}