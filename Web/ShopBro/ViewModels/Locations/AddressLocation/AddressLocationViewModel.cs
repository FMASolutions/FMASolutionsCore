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
            PostCodeFromDB = true;
            PostCodeToCreate = new PostCodeViewModel();
        }   

        public Dictionary<int, string> AvailableCityAreas;
        public Dictionary<int, string> AvailablePostCodes;
        public Int32 AddressLocationID {get; set;}
        public Int32 CityAreaID { get; set; }
        public Int32 PostCodeID { get; set; }
        public string AddressLocationCode { get; set; }
        public string AddressLine1 { get; set; }    
        public string AddressLine2 { get; set; }
        public string StatusMessage { get; set; }
        public bool PostCodeFromDB {get; set;}
        public PostCodeViewModel PostCodeToCreate {get; set;}
    }
}