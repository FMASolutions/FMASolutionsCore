using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class DTOOrderItemDetailed
    {
        public int OrderHeaderID {get; set;}
        public int OrderItemID {get; set;}
        public string OrderItemDescription {get;set;}
        public int OrderItemQty {get; set;}
        public decimal OrderItemUnitPrice {get;set;}
        public decimal OrderItemUnitPriceAfterDiscount {get; set;}
        public int OrderItemStatusID {get;set;}
        public string OrderItemStatusValue {get;set;}        
        public int ItemID {get;set;}
        public string ItemCode {get;set;}
        public string ItemImageFilename {get;set;}
        public int SubGroupID {get;set;}
        public string SubGroupCode {get;set;}
        public string SubGroupName {get;set;}
        public string SubGroupDescription {get;set;}
        public int ProductGroupID {get;set;}
        public string ProductGroupCode {get;set;}
        public string ProductGroupName {get;set;}
        public string ProductGroupDescription {get;set;}        
    }
}