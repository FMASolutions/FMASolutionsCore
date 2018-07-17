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

        [DisplayName(" ID ( 1 - 9,999,999 ) ")]
        public Int32 CustomerTypeID { get; set; }

        [Required(ErrorMessage = "Customer Type Code Is Required")]
        [StringLength(5, ErrorMessage = "Customer Type Code should be no more than 5 Characters")]
        public string CustomerTypeCode { get; set; }

        [Required(ErrorMessage = "Customer Type Name is mandatory")]
        [StringLength(100, ErrorMessage = "Customer Type Name should be no more than 100 characters")]
        public string CustomerTypeName { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}