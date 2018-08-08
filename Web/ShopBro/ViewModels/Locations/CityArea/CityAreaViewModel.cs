using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CityAreaViewModel
    {
        public CityAreaViewModel()
        {            
            AvailableCities = new Dictionary<int,string>();
        }   

        public Dictionary<int, string> AvailableCities;
        public Int32 CityAreaID {get; set;}
        public Int32 CityID { get; set; }
        public string CityAreaCode { get; set; }
        public string CityAreaName { get; set; }    
        public string StatusMessage { get; set; }
    }
}