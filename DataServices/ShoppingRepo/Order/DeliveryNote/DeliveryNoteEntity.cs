using System;
using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class DeliveryNoteEntity : IBaseEntity
    {
        public class DeliveryNoteItem
        {
            public Int32 DeliveryNoteItemID {get; set;}
            public Int32 DeliveryNoteID {get; set;}
            public Int32 OrderItemID {get; set;}
        }
        
        public DeliveryNoteEntity()
        {
            _deliveryNoteItems = new List<DeliveryNoteItem>();
            _deliveryDate = new DateTime();
        }
        public DeliveryNoteEntity(Int32 deliveryNoteID, Int32 orderHeaderID, DateTime deliveryDate, List<DeliveryNoteItem> items)
        {
            _deliveryNoteID = deliveryNoteID;
            _orderHeaderID = orderHeaderID;
            _deliveryDate = deliveryDate;
            _deliveryNoteItems = items;
        }

        protected Int32 _deliveryNoteID;
        protected Int32 _orderHeaderID;
        protected DateTime _deliveryDate;
        protected List<DeliveryNoteItem> _deliveryNoteItems;
        
        public Int32 ID { get { return _deliveryNoteID; } set { _deliveryNoteID = value; } }
        public Int32 DeliveryNoteID { get { return _deliveryNoteID; } set { _deliveryNoteID = value; } }     
        public Int32 OrderHeaderID { get { return _orderHeaderID;} set { _orderHeaderID = value; } }
        public DateTime DeliveryDate { get { return _deliveryDate; } set { _deliveryDate = value; } }
        public List<DeliveryNoteItem> Items {get { return _deliveryNoteItems } set { _deliveryNoteItems = value; } }
    }
}
