using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderItemCreationDTO
    {
        public int OrderHeaderID {get;set;}
        public int ItemID {get;set;}
        public int OrderItemStatusID {get;set;}
        public decimal OrderItemUnitPrice {get;set;}
        public decimal OrderItemUnitPriceAfterDiscount {get;set;}
        public int OrderItemQty{get;set;}
        public string OrderItemDescription {get;set;}
    }
}