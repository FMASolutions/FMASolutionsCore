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

        [DisplayName("City Area ID (1 - 9,999,999")]
        public Int32 CityAreaID {get; set;}

        [DisplayName("City ID ( 1 - 9,999,999 ) ")]
        public Int32 CityID { get; set; }

        [Required(ErrorMessage = "City Code Is Required")]
        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string CityAreaCode { get; set; }

        [Required(ErrorMessage = "City Name is mandatory")]
        [StringLength(100, ErrorMessage = "City Name should be no more than 100 characters")]
        public string CityAreaName { get; set; }    
        
        public string StatusErrorMessage { get; set; }
    }
}