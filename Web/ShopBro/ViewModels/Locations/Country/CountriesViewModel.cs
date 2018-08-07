using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CountriesViewModel
    {
        public CountriesViewModel()
        {
            Countries = new List<CountryViewModel>();
        }
        
        public List<CountryViewModel> Countries { get; set; }
        public string StatusMessage { get; set; }
    }
}