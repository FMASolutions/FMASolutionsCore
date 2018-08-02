using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerSearchViewModel
    {
        public CustomerSearchViewModel()
        {
            this.CustomerID = 0;
            this.CustomerCode = "";
        }

        public Int32 CustomerID { get; set; }

        [StringLength(5, ErrorMessage = "Code should be no more than 5 Characters")]
        public string CustomerCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}