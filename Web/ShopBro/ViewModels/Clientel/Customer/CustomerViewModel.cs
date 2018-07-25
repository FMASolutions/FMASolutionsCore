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

        [DisplayName("Customer ID ( 1 - 9,999,999 ) ")]
        public Int32 CustomerID { get; set; }

        [Range(1, 9999999, ErrorMessage = "Customer Type Should be between 1 and 9,999,999")]
        [DisplayName("Customer Type ID ( 1 - 9,999,999 ) ")]
        public Int32 CustomerTypeID { get; set; }

        [Required(ErrorMessage = "Customer Code is Required")]
        [StringLength(5, ErrorMessage = "Customer Code should be no more than 5 Characters")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Customer Name is Required")]
        [StringLength(250, ErrorMessage = "Customer Name should be no more than 250 Characters")]
        public string CustomerName { get; set; }


        [StringLength(250, ErrorMessage = "Customer Email Address should be no more than 250 Characters")]
        public string CustomerEmailAddress { get; set; }

        [Required(ErrorMessage = "Customer Contact Number is Required")]
        [StringLength(30, ErrorMessage = "Customer Contact Number should be no more than 30 Characters")]
        public string CustomerContactNumber { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}