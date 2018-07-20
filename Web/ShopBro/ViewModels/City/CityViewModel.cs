using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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

        [DisplayName("City ID (1 - 9,999,999")]
        public Int32 CityID {get; set;}

        [DisplayName(" Country ID ( 1 - 9,999,999 ) ")]
        public Int32 CountryID { get; set; }

        [Required(ErrorMessage = "City Code Is Required")]
        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "City Name is mandatory")]
        [StringLength(100, ErrorMessage = "City Name should be no more than 100 characters")]
        public string CityName { get; set; }    
        
        public string StatusErrorMessage { get; set; }
    }
}