using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class OrderItem : OrderItemEntity, IModel
    {
        public OrderItem(ICustomModelState modelState, int itemID, int orderItemStatusID, int orderHeaderID, string orderItemDescritpion, int orderItemID, int orderItemQty, decimal orderItemUnitPrice, decimal orderItemUnitPriceAfterDiscount)
        {
            this._modelState = modelState;

            this._itemID = itemID;
            this._orderHeaderID = orderHeaderID;
            this._orderItemDescription = orderItemDescritpion;
            this._orderItemID = orderItemID;
            this._orderItemQty = orderItemQty;
            this._orderItemUnitPrice = orderItemUnitPrice;
            this._orderItemStatusID = orderItemStatusID;
            this._orderItemUnitPriceAfterDiscount = orderItemUnitPriceAfterDiscount;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}