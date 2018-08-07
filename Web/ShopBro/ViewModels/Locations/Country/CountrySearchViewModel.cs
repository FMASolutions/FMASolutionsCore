using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CountrySearchViewModel
    {
        public CountrySearchViewModel()
        {
            this.CountryID = 0;
            this.CountryCode = "";
        }

        public Int32 CountryID { get; set; }

        [StringLength(5, ErrorMessage = "Country Code should be no more than 5 Characters")]
        public string CountryCode { get; set; }

        public string StatusMessage { get; set; }
    }
}