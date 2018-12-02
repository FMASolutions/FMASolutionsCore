using System;
namespace FMASolutionsCore.BusinessServices.ShoppingDTOFactory
{
    public class OrderPreviewDTO
    {
        public int OrderID {get;set;}
        public string OrderStatus {get;set;}
        public string Customer {get;set;}
        public DateTime OrderDate {get;set;}
        public DateTime OrderDueDate {get;set;}
    }
}