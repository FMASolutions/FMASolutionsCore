using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerTypeSearchViewModel
    {
        public CustomerTypeSearchViewModel()
        {
            this.CustomerTypeID = 0;
            this.CustomerTypeCode = "";
        }

        public Int32 CustomerTypeID { get; set; }

        [StringLength(5, ErrorMessage = "Customer Type Code should be no more than 5 Characters")]
        public string CustomerTypeCode { get; set; }

        public string StatusMessage { get; set; }
    }
}