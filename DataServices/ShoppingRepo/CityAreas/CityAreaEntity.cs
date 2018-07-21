using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityAreaEntity : IBaseEntity
    {
        public CityAreaEntity()
        {
            
        }
        public CityAreaEntity(Int32 cityAreaID, Int32 cityID, string cityAreaCode, string cityAreaName)
        {            
            _cityID = cityID;
            _cityAreaID = cityAreaID;
            _cityAreaCode = cityAreaCode;
            _cityAreaName = cityAreaName;            
        }

        protected Int32 _cityAreaID;
        protected Int32 _cityID;
        protected string _cityAreaCode;
        protected string _cityAreaName;        
        public Int32 ID { get { return _cityAreaID; } set { _cityAreaID = value; } }
        public Int32 CityAreaID { get { return _cityAreaID; } set { _cityAreaID = value; } }
        public Int32 CityID { get { return _cityID; } set { _cityID = value; } }        
        public string CityAreaName { get => _cityAreaName; set => _cityAreaName = value; }
        public string CityAreaCode { get => _cityAreaCode; set => _cityAreaCode = value; }
    }
}