using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class DeliveryNote : DeliveryNoteEntity, IModel
    {
        public SubGroup(ICustomModelState modelState, int deliveryNoteID = 0, int orderHeaderID = 0, datetime deliveryDate = DateTime.Now, List<DeliveryNoteItem> items = new List<DeliveryNoteItem>())
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
