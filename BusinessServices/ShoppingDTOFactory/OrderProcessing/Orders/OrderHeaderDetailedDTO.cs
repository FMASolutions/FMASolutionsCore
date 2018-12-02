using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderHeaderDetailedDTO
    {
        public int OrderID {get; set;}
        public DateTime OrderDate {get; set;}
        public DateTime OrderDueDate {get; set;}
        public int OrderStatusID {get; set;}
        public string OrderStatusValue {get; set;}
        public int CustomerID {get;set;}
        public string CustomerCode {get;set;}
        public string CustomerName {get;set;}
        public string CustomerContactNumber {get;set;}
        public string CustomerEmailAddress {get;set;}
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
    }
}