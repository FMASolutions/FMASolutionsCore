using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class ItemEntity : IBaseEntity
    {
        public ItemEntity()
        {
            
        }
        public ItemEntity(Int32 itemID, Int32 subGroupID, string itemCode, string itemName, string itemDescription
            , decimal itemUnitPrice, decimal itemUnitPriceWithMaxDiscount, Int32 itemAvailableQty
            , Int32 itemReorderQtyReminder, string itemImageFilename
        )
        {
            _itemID = itemID;
            _subGroupID = subGroupID;
            _itemCode = itemCode;
            _itemName = itemName;
            _itemDescription = itemDescription;
            _itemUnitPrice = itemUnitPrice;
            _itemUnitPriceWithMaxDiscount = itemUnitPriceWithMaxDiscount;
            _itemAvailableQty = itemAvailableQty;
            _itemReorderQtyReminder = itemReorderQtyReminder;
            _itemImageFilename = itemImageFilename;
        }
        protected Int32 _itemID;
        protected Int32 _subGroupID;
        protected string _itemCode;
        protected string _itemName;
        protected string _itemDescription;
        protected decimal _itemUnitPrice;
        protected decimal _itemUnitPriceWithMaxDiscount;
        protected Int32 _itemAvailableQty;
        protected Int32 _itemReorderQtyReminder;
        protected string _itemImageFilename;

        public Int32 ID { get { return _itemID; } set { _itemID = value; } }
        public Int32 ItemID { get { return _itemID; } set { _itemID = value; } }
        public Int32 SubGroupID { get { return _subGroupID; } set { _subGroupID = value; } }
        public string ItemCode { get => _itemCode; set => _itemCode = value; }
        public string ItemName { get => _itemName; set => _itemName = value; }
        public string ItemDescription { get => _itemDescription; set => _itemDescription = value; }
        public decimal ItemUnitPrice { get => _itemUnitPrice; set => _itemUnitPrice = value; }
        public decimal ItemUnitPriceWithMaxDiscount { get => _itemUnitPriceWithMaxDiscount; set => _itemUnitPriceWithMaxDiscount = value; }
        public Int32 ItemAvailableQty { get => _itemAvailableQty; set => _itemAvailableQty = value; }
        public Int32 ItemReorderQtyReminder { get => _itemReorderQtyReminder; set => _itemReorderQtyReminder = value; }
        public string ItemImageFilename { get => _itemImageFilename; set => _itemImageFilename = value; }
    }
}