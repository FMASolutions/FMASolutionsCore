using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CreateOrderViewModel
    {
        public CreateOrderViewModel()
        {
            AvailableCustomers = new Dictionary<int, string>();
            AvailableAddresses = new Dictionary<int, string>();
            UseExistingAddress = true;
        }
        public string StatusMessage {get;set;}
        public Dictionary<int, string> AvailableCustomers {get;set;}
        public int SelectedCustomerID {get;set;}
        public bool UseExistingAddress {get; set;}
        public Dictionary<int, string> AvailableAddresses {get;set;}
        public int SelectedAddressID {get;set;}
        public AddressLocationViewModel NewAddressLocationVM {get;set;}
        public DateTime OrderDate {get;set;}
        public DateTime OrderDeliveryDueDate {get;set;}
    }
}