using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CitiesViewModel
    {
        public CitiesViewModel()
        {
            Cities = new List<CityViewModel>();
        }
        
        public List<CityViewModel> Cities { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}