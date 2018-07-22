using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerEntity : IBaseEntity
    {
        public CustomerEntity()
        {

        }
        public CustomerEntity(Int32 customerID, Int32 customerTypeID, string customerCode, string customerName, string customerContactNumber, string customerEmailAddress)
        {
            _customerID = customerID;
            _customerTypeID = customerTypeID;
            _customerCode = customerCode;
            _customerName = customerName;
            _customerContactNumber = customerContactNumber;
            _customerEmailAddress = customerEmailAddress;

        }

        protected Int32 _customerID;
        protected Int32 _customerTypeID;
        protected string _customerCode;
        protected string _customerName;
        protected string _customerContactNumber;
        protected string _customerEmailAddress;
        public Int32 ID { get { return _customerID; } set { _customerID = value; } }
        public Int32 CustomerID { get { return _customerID; } set { _customerID = value; } }
        public Int32 CustomerTypeID { get { return _customerTypeID; } set { _customerTypeID = value; } }
        public string CustomerContactNumber { get => _customerContactNumber; set => _customerContactNumber = value; }
        public string CustomerEmailAddress { get => _customerEmailAddress; set => _customerEmailAddress = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string CustomerCode { get => _customerCode; set => _customerCode = value; }
    }
}