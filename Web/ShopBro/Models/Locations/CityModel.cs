using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CityModel : IModel, IDisposable
    {
        public CityModel(ICustomModelState modelState, ICityService service)
        {
            _modelState = modelState;
            _service = service;
        }
        public void Dispose()
        {
            _service.Dispose();
        }
        private ICustomModelState _modelState;
        private ICityService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public CityViewModel GetemptyViewModel()
        {
            return ConvertToViewModel(new City(_modelState));
        }
        public CityViewModel Search(int id = 0, string code = "")
        {
            City searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CityViewModel returnVM = new CityViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }

        public CitiesViewModel GetAllCities()
        {
            List<City> cityList = _service.GetAll();
            CitiesViewModel vmReturn = new CitiesViewModel();

            if (cityList != null && cityList.Count > 0)
            {
                foreach (City item in cityList)
                {
                    vmReturn.Cities.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Cities Found";
            return vmReturn;
        }

        public CityViewModel Create(CityViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            City model = ConvertToModel(vmUserInput);
            CityViewModel vmResult = ConvertToViewModel(model);

            _modelState = model.ModelState;

            if (_service.CreateNew(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Create Complete.";
            }
            else
            {
                vmUserInput.StatusMessage = "Create Failed: ";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
        }

        public CityViewModel UpdateDB(CityViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            City model = ConvertToModel(vmUserInput);
            CityViewModel vmResult = ConvertToViewModel(model);
            if (_service.UpdateDB(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Update Complete.";
            }
            else
            { 
                vmResult.StatusMessage = "Update Failed: ";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
        }

        private Dictionary<int, string> GetAvailableCountries()
        {
            Dictionary<int, string> countries = new Dictionary<int, string>();
            var list = _service.GetAvailableCountries();
            if (list != null)
                foreach (var item in list)
                    countries.Add(item.CountryID, item.CountryID.ToString() + " (" + item.CountryCode + ") - " + item.CountryName);
            return countries;
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