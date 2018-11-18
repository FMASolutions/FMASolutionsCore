using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AddressLocationViewModel
    {
        public AddressLocationViewModel()
        {            
            AvailableCityAreas = new Dictionary<int,string>();
        }   

        public Dictionary<int, string> AvailableCityAreas;
        public Int32 AddressLocationID {get; set;}
        public Int32 CityAreaID { get; set; }
        public string AddressLine1 { get; set; }    
        public string AddressLine2 { get; set; }
        public string PostCode {get; set;}
        public string StatusMessage { get; set; }

    }
}