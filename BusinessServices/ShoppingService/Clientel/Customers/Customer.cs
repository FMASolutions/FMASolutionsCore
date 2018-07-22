using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Customer : CustomerEntity, IModel
    {
        public Customer(ICustomModelState modelState, int customerID = 0, string customerCode = "", int customerTypeID = 0, string customerName = "", string customerContactNumber = "", string customerEmailAddress = "")
        {
            this.ModelState = modelState;
            this._customerID = customerID;
            this._customerCode = customerCode;
            this._customerTypeID = customerTypeID;
            this._customerName = customerName;
            this._customerContactNumber = customerContactNumber;
            this._customerEmailAddress = customerEmailAddress;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}