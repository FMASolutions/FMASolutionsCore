using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CustomerAddress : CustomerAddressEntity, IModel
    {
        public CustomerAddress(ICustomModelState modelState, int customerAddressID = 0, int customerID = 0, int addressLocationID = 0, bool isDefaultAddress = true, string customerAddressDescription = "")
        {
            this.ModelState = modelState;            
            this._customerAddressID = customerAddressID;            
            this._customerID = customerID;
            this._addressLocationID = addressLocationID;
            this._isDefaultAddress = isDefaultAddress;
            this._customerAddressDescription = customerAddressDescription;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}