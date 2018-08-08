using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            AvailableCustomerTypes = new Dictionary<int, string>();
        }

        public Dictionary<int, string> AvailableCustomerTypes;
        public Int32 CustomerID { get; set; }
        public Int32 CustomerTypeID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerContactNumber { get; set; }
        public string StatusMessage { get; set; }
    }
}