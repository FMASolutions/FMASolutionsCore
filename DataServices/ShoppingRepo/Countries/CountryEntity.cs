using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CountryEntity : IBaseEntity
    {
        public CountryEntity()
        {
            
        }
        public CountryEntity(Int32 countryID, string countryCode, string countryName)
        {
            _countryID = countryID;
            _countryCode = countryCode;
            _countryName = countryName;
        }
        protected Int32 _countryID;
        protected string _countryCode;
        protected string _countryName;
        public Int32 ID { get { return _countryID; } set { _countryID = value; } }
        public Int32 CountryID { get { return _countryID; } set { _countryID = value; } }
        public string CountryCode { get { return _countryCode; } set { _countryCode = value; } }
        public string CountryName { get { return _countryName; } set { _countryName = value; } }
    }
}