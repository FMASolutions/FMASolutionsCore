using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CustomerType : CustomerTypeEntity, IModel
    {
        public CustomerType(ICustomModelState modelState, int customerTypeID = 0, string customerTypeCode = "", string customerTypeName = "")
        {
            _modelState = modelState;
            _customerTypeID = customerTypeID;
            _customerTypeCode = customerTypeCode;
            _customerTypeName = customerTypeName;
        }
        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}