using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Item : ItemEntity, IModel
    {
        public Item(ICustomModelState modelState, int itemID, string itemCode, int subGroupID, string itemName, string itemDescription, decimal itemUnitPrice, decimal itemUnitPriceWithMaxDiscount
        , int itemAvailableQty, int itemReorderQtyReminder, string itemImageLocation)
        {
            _modelState = modelState;
            _itemID = itemID;
            _itemCode = itemCode;
            _subGroupID = subGroupID;
            _itemName = itemName;
            _itemDescription = itemDescription;
            _itemUnitPrice = itemUnitPrice;
            _itemUnitPriceWithMaxDiscount = itemUnitPriceWithMaxDiscount;
            _itemAvailableQty = itemAvailableQty;
            _itemReorderQtyReminder = itemReorderQtyReminder;
            _itemImageFilename = itemImageLocation;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}