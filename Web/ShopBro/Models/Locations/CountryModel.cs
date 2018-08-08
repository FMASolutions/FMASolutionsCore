using System;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CountryModel : IModel, IDisposable
    {
        public CountryModel(ICustomModelState modelState, ICountryService countryService)
        {
            _modelState = modelState;
            _service = countryService;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private ICountryService _service;
        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }

        public CountryViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new Country(_modelState));
        }
        public CountryViewModel Search(int id = 0, string code = "")
        {
            Country searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CountryViewModel returnVM = new CountryViewModel();
                returnVM.StatusMessage = "No result found";
                return returnVM;
            }
        }
        public CountriesViewModel GetAllCountries()
        {
            List<Country> countryList = _service.GetAll();
            CountriesViewModel vmReturn = new CountriesViewModel();

            if (countryList != null && countryList.Count > 0)
            {
                foreach (Country item in countryList)
                {
                    vmReturn.Countries.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Product Groups Found";
            return vmReturn;
        }

        public CountryViewModel Create(CountryViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            Country model = ConvertToModel(vmUserInput);
            CountryViewModel vmResult = ConvertToViewModel(model);

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

        public CountryViewModel UpdateDB(CountryViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            Country model = ConvertToModel(vmUserInput);
            CountryViewModel vmResult = ConvertToViewModel(model);
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

        public CountryViewModel CountryViewModel(CountryViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            Country model = ConvertToModel(vmUserInput);
            CountryViewModel vmResult = ConvertToViewModel(model);
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

        private CountryViewModel ConvertToViewModel(Country sourceModel)
        {
            CountryViewModel model = new CountryViewModel();
            model.CountryID = sourceModel.CountryID;
            model.CountryCode = sourceModel.CountryCode;
            model.CountryName = sourceModel.CountryName;            
            return model;
        }

        private Country ConvertToModel(CountryViewModel vm)
        {
            Country country = new Country(_modelState
                , vm.CountryID
                , vm.CountryCode
                , vm.CountryName                
            );
            return country;
        }
    }
}