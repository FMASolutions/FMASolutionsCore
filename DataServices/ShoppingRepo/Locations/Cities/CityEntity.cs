using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityEntity : IBaseEntity
    {
        public CityEntity()
        {
            
        }
        public CityEntity(Int32 cityID, Int32 countryID, string cityCode, string cityName)
        {            
            _cityID = cityID;
            _countryID = countryID;
            _cityCode = cityCode;
            _cityName = cityName;            
        }

        protected Int32 _cityID;
        protected Int32 _countryID;
        protected string _cityCode;
        protected string _cityName;        
        public Int32 ID { get { return _cityID; } set { _cityID = value; } }
        public Int32 CityID { get { return _cityID; } set { _cityID = value; } }
        public Int32 CountryID { get { return _countryID; } set { _countryID = value; } }        
        public string CityName { get => _cityName; set => _cityName = value; }
        public string CityCode { get => _cityCode; set => _cityCode = value; }
    }
}