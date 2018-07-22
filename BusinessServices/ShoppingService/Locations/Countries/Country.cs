using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Country : CountryEntity, IModel
    {
        public Country(ICustomModelState modelState, int countryID = 0, string countryCode = "", string countryName = "")
        {
            _modelState = modelState;
            _countryID = countryID;
            _countryCode = countryCode;
            _countryName = countryName;
        }
        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}