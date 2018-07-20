using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CityModel : IModel
    {
        public CityModel(ICustomModelState modelState, ICityService service)
        {
            _modelState = modelState;
            _cityService = service;
        }
        private ICustomModelState _modelState;
        private ICityService _cityService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public CityViewModel Search(int id = 0, string code = "")
        {
            City searchResult = null;
            if (id > 0)
                searchResult = _cityService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _cityService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CityViewModel returnVM = new CityViewModel();
                returnVM.StatusErrorMessage = "No result Found";
                return returnVM;
            }
        }

        public CitiesViewModel GetAllCities()
        {
            List<City> cityList = _cityService.GetAll();
            CitiesViewModel vmReturn = new CitiesViewModel();

            if (cityList != null && cityList.Count > 0)
            {
                foreach (City item in cityList)
                {
                    vmReturn.Cities.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No Cities Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableCountries()
        {
            Dictionary<int, string> countries = new Dictionary<int, string>();
            var list = _cityService.GetAvailableCountries();
            if (list != null)
                foreach (var item in list)
                    countries.Add(item.CountryID, item.CountryID.ToString() + " (" + item.CountryCode + ") - " + item.CountryName);
            return countries;
        }

        public CityViewModel Create(CityViewModel newCity)
        {
            City city = ConvertToModel(newCity);
            city.CityID = 0; //NOT SURE I NEED TRHIS???????????????
            CityViewModel vmReturn = new CityViewModel();
            if (_cityService.CreateNew(city))
                vmReturn = ConvertToViewModel(city);
            else
            {
                vmReturn.StatusErrorMessage = "Unable to create City";
                foreach (string item in city.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusErrorMessage += " " + item;
            }
            return vmReturn;
        }

        public bool UpdateDB(CityViewModel updatedCity)
        {
            City city = ConvertToModel(updatedCity);
            if (_cityService.UpdateDB(city))
                return true;
            else
                _modelState = city.ModelState;
            return false;
        }
        private CityViewModel ConvertToViewModel(City model)
        {
            CityViewModel vm = new CityViewModel();
            vm.CountryID = model.CountryID;
            vm.CityID = model.CityID;
            vm.CityCode = model.CityCode;
            vm.CityName = model.CityName;            
            vm.AvailableCountries = GetAvailableCountries();
            return vm;
        }

        private City ConvertToModel(CityViewModel vm)
        {
            City city = new City(_modelState
                , vm.CityID
                , vm.CityCode
                , vm.CountryID
                , vm.CityName                
            );
            return city;
        }
    }
}