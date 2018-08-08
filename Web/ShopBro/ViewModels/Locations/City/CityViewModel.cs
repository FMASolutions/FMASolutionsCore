using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CityViewModel
    {
        public CityViewModel()
        {            
            AvailableCountries = new Dictionary<int,string>();
        }   

        public Dictionary<int, string> AvailableCountries;
        public Int32 CityID {get; set;}
        public Int32 CountryID { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }    
        
        public string StatusMessage { get; set; }
    }
}