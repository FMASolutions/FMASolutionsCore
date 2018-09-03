using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class AddressLocationEntity : IBaseEntity
    {
        public AddressLocationEntity()
        {
            
        }
        public AddressLocationEntity(Int32 addressLocationID, Int32 cityAreaID, string addressLocationCode, string addressLine1, string addressLine2,string postCode)
        {            
            _addressLocationID = addressLocationID;
            _cityAreaID = cityAreaID;
            _postCode = postCode;
            _addressLocationCode = addressLocationCode;
            _addressLine1 = addressLine1;            
            _addressLine2 = addressLine2;
        }

        protected Int32 _addressLocationID;
        protected Int32 _cityAreaID;
        protected string _postCode;
        protected string _addressLocationCode;
        protected string _addressLine1;
        protected string _addressLine2;             
        public Int32 ID { get { return _addressLocationID; } set { _addressLocationID = value; } }
        public Int32 AddressLocationID { get { return _addressLocationID; } set { _addressLocationID = value; } }
        public Int32 CityAreaID { get { return _cityAreaID; } set { _cityAreaID = value; } }        
        public string PostCode { get { return _postCode; } set { _postCode = value; } }        
        public string AddressLocationCode { get => _addressLocationCode; set => _addressLocationCode = value; }
        public string AddressLine1 { get => _addressLine1; set => _addressLine1 = value; }
        public string AddressLine2 { get => _addressLine2; set => _addressLine2 = value; }
    }
}