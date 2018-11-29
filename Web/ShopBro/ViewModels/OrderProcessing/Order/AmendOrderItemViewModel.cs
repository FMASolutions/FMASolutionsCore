using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AmendOrderItemViewModel
    {
        public int OrderHeaderID {get; set;}
        public DateTime OrderDate {get; set;}
        public DateTime OrderDueDate {get; set;}
        public int OrderStatusID {get; set;}
        public string OrderStatusValue {get; set;}
        public int OrderItemID {get; set;}
        public string OrderItemDescription {get;set;}
        public int OrderItemQty {get; set;}
        public decimal OrderItemUnitPrice {get;set;}
        public decimal OrderItemUnitPriceAfterDiscount {get; set;}
        public int OrderItemStatusID {get;set;}
        public string OrderItemStatusValue {get;set;}
        public int CustomerID {get;set;}
        public string CustomerCode {get;set;}
        public string CustomerName {get;set;}
        public string CustomerContactNumber {get;set;}
        public string CustomerEmailAddress {get;set;}
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
        public int CustomerAddressID {get;set;}
        public string CustomerAddressDescription {get;set;}
        public bool IsDefaultAddress {get;set;}
        public int AddressLocationID {get;set;}
        public string AddressLine1 {get;set;}
        public string AddressLine2 {get;set;}
        public string AddressPostCode {get;set;}
        public int CityAreaID {get;set;}
        public string CityAreaCode {get;set;}
        public string CityAreaName {get;set;}
        public int CityID {get;set;}
        public string CityCode {get;set;}
        public string CityName {get;set;}
        public int CountryID {get;set;}
        public string CountryCode {get;set;}
        public string CountryName {get;set;}        
        public string StatusMessage {get; set;}
    }
}