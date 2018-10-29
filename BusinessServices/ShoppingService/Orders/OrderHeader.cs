using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
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
}