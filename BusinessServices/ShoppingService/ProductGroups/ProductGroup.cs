using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
namespace FMASolutionsCore.BusinessServices.ShoppingService.ProductGroups
{
    public class ProductGroup : ProductGroupEntity, IModel
    {
        public ProductGroup(ICustomModelState modelState,int productGroupID = 0, string productGroupCode = "", string productGroupName = "", string productGroupDescription = "")
        {          
            this.ModelState = modelState;
            this._productGroupID = productGroupID;
            this._productGroupCode = productGroupCode;
            this._productGroupName = productGroupName;
            this._productGroupDescription = productGroupDescription;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}