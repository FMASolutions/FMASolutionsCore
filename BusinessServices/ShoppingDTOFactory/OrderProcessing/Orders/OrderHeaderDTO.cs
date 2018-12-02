using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderHeaderDTO
    {
        public int OrderID {get;set;}
        public DateTime OrderDate {get;set;}
        public DateTime OrderDueDate {get;set;}
        public string OrderStatus {get;set;}
        public string CustomerName {get;set;}
        public string AddressLine1 {get;set;}
        public string AddressLine2 {get;set;}
        public string CityArea {get;set;}
        public string City {get;set;}
        public string PostCode {get;set;}
        public string Country {get;set;}
    }
}