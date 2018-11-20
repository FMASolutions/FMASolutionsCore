using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class DeliveryNote : DeliveryNoteEntity, IModel
    {
        public DeliveryNote(ICustomModelState modelState, List<DeliveryNoteItem> items, int deliveryNoteID = 0, int orderHeaderID = 0, DateTime deliveryDate = new DateTime())
        {
            this.ModelState = modelState;
            this._deliveryNoteID = deliveryNoteID;
            this._orderHeaderID = orderHeaderID;
            this._deliveryDate = deliveryDate;
            this._deliveryNoteItems = items;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}
