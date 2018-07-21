using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CityAreasViewModel
    {
        public CityAreasViewModel()
        {
            CityAreas = new List<CityAreaViewModel>();
        }
        
        public List<CityAreaViewModel> CityAreas { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}