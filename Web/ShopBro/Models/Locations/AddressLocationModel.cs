using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class AddressLocationModel : IModel, IDisposable
    {
        public AddressLocationModel(ICustomModelState modelState, IAddressLocationService service)
        {
            _modelState = modelState;
            _addressLocationService = service;            
        }
        public void Dispose()
        {
            _addressLocationService.Dispose();
        }
        private ICustomModelState _modelState;
        private IAddressLocationService _addressLocationService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public AddressLocationViewModel Search(int id = 0, string code = "")
        {

            AddressLocation searchResult = null;
            if (id > 0)
                searchResult = _addressLocationService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _addressLocationService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                AddressLocationViewModel returnVM = new AddressLocationViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }

        public AddressLocationsViewModel GetAllAddressLocations()
        {            
            List<AddressLocation> addressLocationList = _addressLocationService.GetAll();
            AddressLocationsViewModel vmReturn = new AddressLocationsViewModel();

            if (addressLocationList != null && addressLocationList.Count > 0)
            {
                foreach (AddressLocation item in addressLocationList)
                {
                    vmReturn.AddressLocations.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Address Locations Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableCityAreas()
        {
            Dictionary<int, string> cityAreas = new Dictionary<int, string>();
            var list = _addressLocationService.GetAvailableCityAreas();
            if (list != null)
                foreach (var item in list)
                    cityAreas.Add(item.CityAreaID, item.CityAreaID.ToString() + " (" + item.CityAreaCode + ") - " + item.CityAreaName);
            return cityAreas;
        }
        public Dictionary<int, string> GetAvailablePostCodes()
        {
            Dictionary<int, string> postCodes = new Dictionary<int, string>();
            var list = _addressLocationService.GetAvailablePostCodes();
            if (list != null)
                foreach (var item in list)
                    postCodes.Add(item.PostCodeID, item.PostCodeID.ToString() + " (" + item.PostCodeCode + ") - " + item.PostCodeValue);
            return postCodes;
        }

        public Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _addressLocationService.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }
        public AddressLocationViewModel Create(AddressLocationViewModel newAddressLocation)
        {
            AddressLocation addressLocation = ConvertToModel(newAddressLocation);
            addressLocation.AddressLocationID = 0; //NOT SURE I NEED TRHIS???????????????
            AddressLocationViewModel vmReturn = new AddressLocationViewModel();

            if(newAddressLocation.postCodeFromDB == false)
            {
                PostCode postCode = new PostCode(_modelState, 0, newAddressLocation.postCodeToCreate.PostCodeCode, newAddressLocation.postCodeToCreate.CityID, newAddressLocation.postCodeToCreate.PostCodeValue);
                if(_addressLocationService.CreateNew(addressLocation, postCode))
                {
                    vmReturn = ConvertToViewModel(addressLocation);
                }
                else
                {
                    vmReturn.StatusMessage = "Unable to create Address Locations";
                    foreach (string item in addressLocation.ModelState.ErrorDictionary.Values)
                        vmReturn.StatusMessage += " " + item;
                }
            }
            else
            {
                if (_addressLocationService.CreateNew(addressLocation))
                    vmReturn = ConvertToViewModel(addressLocation);
                else
                {
                    vmReturn.StatusMessage = "Unable to create Address Locations";
                    foreach (string item in addressLocation.ModelState.ErrorDictionary.Values)
                        vmReturn.StatusMessage += " " + item;
                }
            }
            return vmReturn;
        }

        public bool UpdateDB(AddressLocationViewModel updatedAddressLocation)
        {
            AddressLocation addressLocation = ConvertToModel(updatedAddressLocation);
            if (_addressLocationService.UpdateDB(addressLocation))
                return true;
            else
                _modelState = addressLocation.ModelState;
            return false;
        }
        private AddressLocationViewModel ConvertToViewModel(AddressLocation model)
        {
            AddressLocationViewModel vm = new AddressLocationViewModel();
            vm.AddressLocationID = model.AddressLocationID;
            vm.AddressLocationCode = model.AddressLocationCode;
            vm.AddressLine1 = model.AddressLine1;
            vm.AddressLine2 = model.AddressLine2;
            vm.CityAreaID = model.CityAreaID;
            vm.PostCodeID = model.PostCodeID;
            vm.AvailableCityAreas = GetAvailableCityAreas();
            vm.AvailablePostCodes = GetAvailablePostCodes();
            return vm;
        }

        private AddressLocation ConvertToModel(AddressLocationViewModel vm)
        {
            AddressLocation addressLocation = new AddressLocation(_modelState
                , vm.AddressLocationID
                , vm.AddressLocationCode
                , vm.CityAreaID
                , vm.PostCodeID
                , vm.AddressLine1
                , vm.AddressLine2
            );
            return addressLocation;
        }
    }
}