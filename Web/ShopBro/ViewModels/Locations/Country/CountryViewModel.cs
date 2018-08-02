using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CountryViewModel
    {
        public CountryViewModel()
        {
            this.CountryID = 0;
            this.CountryCode = "";
            this.CountryName = "";
        }

        public Int32 CountryID { get; set; }

        [StringLength(5, ErrorMessage = "Country Code should be no more than 5 Characters")]
        public string CountryCode { get; set; }

        [StringLength(100, ErrorMessage = "Country Name should be no more than 100 characters")]
        public string CountryName { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}