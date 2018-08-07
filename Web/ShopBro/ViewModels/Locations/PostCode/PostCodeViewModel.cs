using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class PostCodeViewModel
    {
        public PostCodeViewModel()
        {            
            AvailableCities = new Dictionary<int,string>();
        }   

        public Dictionary<int, string> AvailableCities;

        public Int32 PostCodeID {get; set;}

        public Int32 CityID { get; set; }

        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string PostCodeCode { get; set; }

        [StringLength(100, ErrorMessage = "City Name should be no more than 100 characters")]
        public string PostCodeValue { get; set; }    
        
        public string StatusMessage { get; set; }
    }
}