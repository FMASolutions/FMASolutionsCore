using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class AddressLocation : AddressLocationEntity, IModel
    {
        public AddressLocation(ICustomModelState modelState, int addressLocationID = 0, int cityAreaID = 0, string addressLine1 = "", string addressLine2 = "", string postCode = "")
        {
            this.ModelState = modelState;
            this._addressLocationID = addressLocationID;            
            this._cityAreaID = cityAreaID;            
            this._addressLine1 = addressLine1;
            this._addressLine2 = addressLine2;
            this._postCode = postCode;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}