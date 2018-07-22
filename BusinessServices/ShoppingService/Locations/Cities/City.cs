using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class City : CityEntity, IModel
    {
        public City(ICustomModelState modelState, int cityID = 0, string cityCode = "", int countryID = 0, string cityName = "")
        {
            this.ModelState = modelState;
            this._cityID = cityID;
            this._cityCode = cityCode;
            this._countryID = countryID;
            this._cityName = cityName;            
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}