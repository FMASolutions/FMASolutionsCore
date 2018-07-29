using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AddressLocationViewModel
    {
        public AddressLocationViewModel()
        {            
            AvailableCityAreas = new Dictionary<int,string>();
            AvailablePostCodes = new Dictionary<int,string>();





            postCodeFromDB = true;
            postCodeToCreate = new PostCodeViewModel();
        }   

        public Dictionary<int, string> AvailableCityAreas;
        public Dictionary<int, string> AvailablePostCodes;

        [DisplayName("Address Location ID (1 - 9,999,999")]
        public Int32 AddressLocationID {get; set;}

        [DisplayName(" City Area ID ( 1 - 9,999,999 ) ")]
        public Int32 CityAreaID { get; set; }
        
        [DisplayName(" Postcode ID ( 1 - 9,999,999 ) ")]
        public Int32 PostCodeID { get; set; }

        [Required(ErrorMessage = "Address Location Code Is Required")]
        [StringLength(5, ErrorMessage = "Address Location Code should be no more than 5 Characters")]
        public string AddressLocationCode { get; set; }

        [Required(ErrorMessage = "Address Line 1 is mandatory")]
        [StringLength(100, ErrorMessage = "Address Line 1 should be no more than 100 characters")]
        public string AddressLine1 { get; set; }    

        [Required(ErrorMessage = "Address Line 2 is mandatory")]
        [StringLength(100, ErrorMessage = "Address Line 2 should be no more than 100 characters")]
        public string AddressLine2 { get; set; }
        
        public string StatusErrorMessage { get; set; }



        public bool postCodeFromDB {get; set;}
        public PostCodeViewModel postCodeToCreate {get; set;}
    }
}