using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderItemEntity : IBaseEntity
    {
        public OrderItemEntity()
        {
        }
        public OrderItemEntity(Int32 orderItemID, Int32 orderHeaderID, Int32 itemID, decimal orderItemUnitPrice, decimal orderItemUnitPriceAfterDiscount, Int32 orderItemQty, string orderItemDescritpion)
        {
            _orderItemID = orderItemID;
            _orderHeaderID = orderHeaderID;
            _itemID = itemID;
            _orderItemUnitPrice = orderItemUnitPrice;
            _orderItemUnitPriceAfterDiscount = orderItemUnitPriceAfterDiscount;
            _orderItemQty = orderItemQty;
            _orderItemDescription = orderItemDescritpion;
        }

        protected Int32 _orderItemID;
        protected Int32 _orderHeaderID;
        protected Int32 _itemID;
        protected decimal _orderItemUnitPrice;
        protected decimal _orderItemUnitPriceAfterDiscount;
        protected int _orderItemQty;
        protected string _orderItemDescription;
        public Int32 ID { get { return _orderItemID; } set { _orderItemID = value; } }
        public Int32 OrderItemID { get { return _orderItemID; } set { _orderItemID = value; } }
        public Int32 OrderHeaderID { get { return _orderHeaderID; } set { _orderHeaderID = value; } }
        public Int32 ItemID { get { return _itemID; } set { ItemID = value; } }
        public decimal OrderItemUnitPrice { get { return _orderItemUnitPrice; } set { _orderItemUnitPrice = value; } }
        public decimal OrderItemUnitPriceAfterDiscount { get { return _orderItemUnitPriceAfterDiscount; } set { _orderItemUnitPriceAfterDiscount = value; } }
        public int OrderItemQty { get { return _orderItemQty; } set { _orderItemQty = value; } }
        public string OrderItemDescription {get {return _orderItemDescription;} set{ _orderItemDescription = value;}}
    }
}