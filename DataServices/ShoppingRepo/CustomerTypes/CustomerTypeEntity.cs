using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerTypeEntity : IBaseEntity
    {
        public CustomerTypeEntity()
        {
            
        }
        public CustomerTypeEntity(Int32 customerTypeID, string customerTypeCode, string customerTypeName)
        {
            _customerTypeID = customerTypeID;
            _customerTypeCode = customerTypeCode;
            _customerTypeName = customerTypeName;
        }
        protected Int32 _customerTypeID;
        protected string _customerTypeCode;
        protected string _customerTypeName;
        public Int32 ID { get { return _customerTypeID; } set { _customerTypeID = value; } }
        public Int32 CustomerTypeID { get { return _customerTypeID; } set { _customerTypeID = value; } }
        public string CustomerTypeCode { get { return _customerTypeCode; } set { _customerTypeCode = value; } }
        public string CustomerTypeName { get { return _customerTypeName; } set { _customerTypeName = value; } }        
    }
}