using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AddressLocationsViewModel
    {
        public AddressLocationsViewModel()
        {
            AddressLocations = new List<AddressLocationViewModel>();
        }
        
        public List<AddressLocationViewModel> AddressLocations { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}