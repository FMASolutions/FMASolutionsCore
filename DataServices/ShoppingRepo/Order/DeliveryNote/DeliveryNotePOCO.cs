
using System;
using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class DeliveryNotePOCO : IBaseEntity
    {
        public DeliveryNotePOCO()
        {
            
        }
        public DeliveryNotePOCO(Int32 deliveryNoteID, Int32 orderHeaderID, Int32 deliveryNoteItemID ,Int32 orderItemID, DateTime deliveryDate)
        {
            _deliveryNoteID = deliveryNoteID;
            _orderHeaderID = orderHeaderID;
            _deliveryNoteItemID = deliveryNoteItemID;
            _orderItemID = orderItemID;            
            _deliveryDate = deliveryDate;
        }

        protected Int32 _deliveryNoteID;
        protected Int32 _orderHeaderID;  
        protected Int32 _deliveryNoteItemID;
        protected Int32  _orderItemID;
        protected DateTime _deliveryDate;
        
        public Int32 ID { get { return _deliveryNoteID; } set { _deliveryNoteID = value; } }
        public Int32 DeliveryNoteID { get { return _deliveryNoteID; } set { _deliveryNoteID = value; } }     
        public Int32 OrderHeaderID { get { return _orderHeaderID;} set { _orderHeaderID = value; } }
        public Int32 DeliveryNoteItemID { get { return _deliveryNoteItemID; } set { _deliveryNoteItemID = value; } }
        public Int32 OrderItemID { get {return _orderItemID; } set { _orderItemID = value; } }
        public DateTime DeliveryDate { get { return _deliveryDate; } set { _deliveryDate = value; } }
    }
}
