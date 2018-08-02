using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerTypeViewModel
    {
        public CustomerTypeViewModel()
        {
            this.CustomerTypeID = 0;
            this.CustomerTypeCode = "";
            this.CustomerTypeName = "";
        }

        public Int32 CustomerTypeID { get; set; }

        [StringLength(5, ErrorMessage = "Customer Type Code should be no more than 5 Characters")]
        public string CustomerTypeCode { get; set; }

        [StringLength(100, ErrorMessage = "Customer Type Name should be no more than 100 characters")]
        public string CustomerTypeName { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}