using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CityArea : CityAreaEntity, IModel
    {
        public CityArea(ICustomModelState modelState, int cityAreaID = 0, string cityAreaCode = "", int cityID = 0, string cityAreaName = "")
        {
            this.ModelState = modelState;
            this._cityAreaID = cityAreaID;
            this._cityAreaCode = cityAreaCode;
            this._cityID = cityID;
            this._cityAreaName = cityAreaName;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}