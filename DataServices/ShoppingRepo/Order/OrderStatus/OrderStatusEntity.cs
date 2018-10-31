using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderStatusEntity : IBaseEntity
    {
        public OrderStatusEntity()
        {
        }
        public OrderStatusEntity(Int32 orderStatusID, string orderStatusValue)
        {
            _orderStatusID = orderStatusID;
            _orderStatusValue = orderStatusValue;
        }

        protected Int32 _orderStatusID;
        protected string _orderStatusValue;
        public Int32 ID { get { return _orderStatusID; } set { _orderStatusID = value; } }
        public Int32 OrderStatusID { get { return _orderStatusID; } set { _orderStatusID = value; } }
        public string OrderStatusValue { get { return _orderStatusValue; } set { _orderStatusValue = value; } }
        
    }
}