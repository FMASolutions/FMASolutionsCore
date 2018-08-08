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
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StatusMessage { get; set; }
    }
}