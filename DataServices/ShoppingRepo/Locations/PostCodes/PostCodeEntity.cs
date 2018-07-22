using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class PostCodeEntity : IBaseEntity
    {
        public PostCodeEntity()
        {
            
        }
        public PostCodeEntity(Int32 postCodeID, Int32 cityID, string postCodeCode, string postCodeValue)
        {            
            _cityID = cityID;
            _postCodeID = postCodeID;
            _postCodeCode = postCodeCode;
            _postCodeValue = postCodeValue;
        }

        protected Int32 _postCodeID;
        protected Int32 _cityID;
        protected string _postCodeCode;
        protected string _postCodeValue;        
        public Int32 ID { get { return _postCodeID; } set { _postCodeID = value; } }
        public Int32 PostCodeID { get { return _postCodeID; } set { _postCodeID = value; } }
        public Int32 CityID { get { return _cityID; } set { _cityID = value; } }        
        public string PostCodeValue { get => _postCodeValue; set => _postCodeValue = value; }
        public string PostCodeCode { get => _postCodeCode; set => _postCodeCode = value; }
    }
}