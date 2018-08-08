using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerAddressEntity : IBaseEntity
    {
        public CustomerAddressEntity()
        {

        }
        public CustomerAddressEntity(Int32 customerAddressID, Int32 customerID, Int32 addressLocationID, string customerAddressCode, string customerAddressDescription, bool isDefaultAddress = true)
        {
            _customerAddressID = customerAddressID;
            _customerID = customerID;
            _addressLocationID = addressLocationID;
            _customerAddressCode = customerAddressCode;
            _customerAddressDescription = customerAddressDescription;
            _isDefaultAddress = isDefaultAddress;
        }

        protected Int32 _customerAddressID;
        protected Int32 _customerID;
        protected Int32 _addressLocationID;
        protected string _customerAddressCode;
        protected string _customerAddressDescription;
        protected bool _isDefaultAddress;
        public Int32 ID { get { return _customerAddressID; } set { _customerAddressID = value; } }
        public Int32 CustomerAddressID { get { return _customerAddressID; } set { _customerAddressID = value; } }
        public Int32 CustomerID { get { return _customerID; } set { _customerID = value; } }
        public Int32 AddressLocationID { get { return _addressLocationID; } set { _addressLocationID = value; } }
        public string CustomerAddressDescription { get => _customerAddressDescription; set => _customerAddressDescription = value; }
        public string CustomerAddressCode { get => _customerAddressCode; set => _customerAddressCode = value; }
        public bool IsDefaultAddress { get => _isDefaultAddress; set => _isDefaultAddress = value; }
    }
}