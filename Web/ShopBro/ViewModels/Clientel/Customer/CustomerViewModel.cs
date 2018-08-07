using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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

        [StringLength(5, ErrorMessage = "Customer Code should be no more than 5 Characters")]
        public string CustomerCode { get; set; }

        [StringLength(250, ErrorMessage = "Customer Name should be no more than 250 Characters")]
        public string CustomerName { get; set; }


        [StringLength(250, ErrorMessage = "Customer Email Address should be no more than 250 Characters")]
        public string CustomerEmailAddress { get; set; }

        [StringLength(30, ErrorMessage = "Customer Contact Number should be no more than 30 Characters")]
        public string CustomerContactNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}