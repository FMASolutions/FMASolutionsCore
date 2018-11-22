using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AddressLocationViewModel
    {
        public AddressLocationViewModel()
        {            
            AvailableCityAreas = new Dictionary<int,string>();
        }   

        public Dictionary<int, string> AvailableCityAreas;
        public Int32 AddressLocationID {get; set;}
        public Int32 CityAreaID { get; set; }
        public string AddressLine1 { get; set; }    
        public string AddressLine2 { get; set; }
        public string PostCode {get; set;}
        public string AddressFull { get{
            string _fullString = "";
            if(AddressLine1 != "")
                _fullString = AddressLine1;
            
            if(AddressLine2 != "")
                if(_fullString != "")
                    _fullString += ", " + AddressLine2;
                else
                    _fullString = AddressLine2;
            
            if(AvailableCityAreas != null && AvailableCityAreas.Count > 0)
                if(CityAreaID > 0)
                    if(_fullString != "")
                        _fullString += ", " + AvailableCityAreas.GetValueOrDefault(CityAreaID);
            if(PostCode != "")
                if(_fullString != "")
                    _fullString += ", " + PostCode;
                else
                    _fullString = PostCode;            

            return _fullString;
        }}
        public string StatusMessage { get; set; }

    }
}