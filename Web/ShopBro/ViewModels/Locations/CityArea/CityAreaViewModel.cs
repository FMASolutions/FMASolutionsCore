using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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

        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string CityAreaCode { get; set; }

        [StringLength(100, ErrorMessage = "City Name should be no more than 100 characters")]
        public string CityAreaName { get; set; }    
        
        public string StatusErrorMessage { get; set; }
    }
}