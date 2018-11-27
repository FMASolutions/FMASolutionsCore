using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System.Collections.Generic;
using System;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Order
    {
        public Order()
        {
            _items = new List<OrderItem>();
        }
        public OrderHeader Header { get { return _header; } set { _header = value; } }

        public List<OrderItem> OrderItems { get { return _items; } set { _items = value; } }

        private List<OrderItem> _items;
        private OrderHeader _header;
    }
    public class OrderHeader : OrderHeaderEntity, IModel
    {
        public OrderHeader(ICustomModelState modelState, int customerAddressID, int customerID, DateTime deliveryDate, DateTime orderDate, int orderHeaderID, int orderStatusID)
        {
            this._customerAddressID = customerAddressID;
            this._customerID = customerID;
            this._deliveryDate = deliveryDate;
            this._modelState = modelState;
            this._orderDate = orderDate;
            this._orderHeaderID = orderHeaderID;
            this._orderStatusID = orderStatusID;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
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