
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderItemPreviewDTO
    {
        public string OrderItemDescription {get;set;}
        public int OrderItemQty {get;set;}
        public int OrderItemID {get;set;}
        public decimal OrderItemUnitPriceAfterDiscount {get;set;}
        public int ItemID{get;set;}
    }
}