using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingService.SubGroups
{
    public class SubGroup : SubGroupEntity, IModel
    {
        public SubGroup(ICustomModelState modelState, int subGroupID = 0, string subGroupCode = "", int productGroupID = 0, string subGroupName = "", string subGroupDescription = "")
        {
            this.ModelState = modelState;
            this._subGroupID = subGroupID;
            this._subGroupCode = subGroupCode;
            this._productGroupID = productGroupID;
            this._subGroupName = subGroupName;
            this._subGroupDescription = subGroupDescription;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}