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
            _countryService = countryService;
        }
        public void Dispose()
        {
            _countryService.Dispose();
        }
        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }
        private ICustomModelState _modelState;
        private ICountryService _countryService;

        public CountryViewModel Search(int id = 0, string code = "")
        {
            Country searchResult = null;

            if (id > 0)
                searchResult = _countryService.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _countryService.GetByCode(code);
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
            List<Country> countryList = _countryService.GetAll();
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

        public CountryViewModel Create(CountryViewModel newCountry)
        {
            Country country = ConvertToModel(newCountry);
            country.CountryID = 0; //NOT SURE I NEED THIS?????????????
            CountryViewModel vmReturn = new CountryViewModel();
            if (_countryService.CreateNew(country))
                vmReturn = ConvertToViewModel(country);
            else
            {
                vmReturn.StatusMessage = "Unable to create Country";
                foreach (string item in country.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusMessage += item + " ";
            }
            return vmReturn;
        }

        public bool UpdateDB(CountryViewModel updatedCountry)
        {
            Country country = ConvertToModel(updatedCountry);
            if (_countryService.UpdateDB(country))
                return true;
            else
                _modelState = country.ModelState;
            return false;
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