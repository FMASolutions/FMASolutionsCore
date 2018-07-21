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

        [DisplayName("Post Code ID (1 - 9,999,999")]
        public Int32 PostCodeID {get; set;}

        [DisplayName("City ID ( 1 - 9,999,999 ) ")]
        public Int32 CityID { get; set; }

        [Required(ErrorMessage = "Postcode Code Is Required")]
        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string PostCodeCode { get; set; }

        [Required(ErrorMessage = "Post Code Value is mandatory")]
        [StringLength(100, ErrorMessage = "City Name should be no more than 100 characters")]
        public string PostCodeValue { get; set; }    
        
        public string StatusErrorMessage { get; set; }
    }
}