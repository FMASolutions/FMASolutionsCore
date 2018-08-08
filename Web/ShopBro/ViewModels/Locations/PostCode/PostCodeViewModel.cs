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
        public string PostCodeCode { get; set; }
        public string PostCodeValue { get; set; }    
        public string StatusMessage { get; set; }
    }
}