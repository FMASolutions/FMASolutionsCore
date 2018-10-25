using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderHeaderEntity : IBaseEntity
    {
        public OrderHeaderEntity()
        {
            _orderDate = new DateTime();
            _deliveryDate = new DateTime();
        }
        public OrderHeaderEntity(Int32 orderHeaderID, Int32 customerID, Int32 customerAddressID, Int32 orderStatusID, DateTime orderDate, DateTime deliveryDate)
        {
            _orderHeaderID = orderHeaderID;
            _customerID = customerID;
            _customerAddressID = customerAddressID;
            _orderStatusID = orderStatusID;
            _orderDate = orderDate;
            _deliveryDate = deliveryDate;
        }

        protected Int32 _orderHeaderID;
        protected Int32 _customerID;
        protected Int32 _customerAddressID;
        protected Int32 _orderStatusID;
        protected DateTime _orderDate;
        protected DateTime _deliveryDate;
        public Int32 ID { get { return _orderHeaderID; } set { _orderHeaderID = value; } }
        public Int32 OrderHeaderID { get { return _orderHeaderID; } set { _orderHeaderID = value; } }
        public Int32 CustomerID { get { return _customerID; } set { _customerID = value; } }
        public Int32 CustomerAddressID { get { return _customerAddressID; } set { CustomerAddressID = value; } }
        public Int32 OrderStatusID { get { return _orderStatusID; } set { _orderStatusID = value; } }
        public DateTime OrderDate { get { return _orderDate; } set { _orderDate = value; } }
        public DateTime DeliveryDate { get { return _deliveryDate; } set { _deliveryDate = value; } }
    }
}