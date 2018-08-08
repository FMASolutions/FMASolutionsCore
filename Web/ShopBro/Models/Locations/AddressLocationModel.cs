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
            _service = service;            
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private IAddressLocationService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public AddressLocationViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new AddressLocation(_modelState));
        }
        public AddressLocationViewModel Search(int id = 0, string code = "")
        {
            AddressLocation searchResult = null;
            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
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
            List<AddressLocation> addressLocationList = _service.GetAll();
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

        public AddressLocationViewModel Create(AddressLocationViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();
            AddressLocation model = ConvertToModel(vmUserInput);
            AddressLocationViewModel vmResult = ConvertToViewModel(model);

            _modelState = model.ModelState;

            if(vmUserInput.PostCodeFromDB == false)
            {
                PostCode postCode = new PostCode(_modelState, 0, vmUserInput.PostCodeToCreate.PostCodeCode, vmUserInput.PostCodeToCreate.CityID, vmUserInput.PostCodeToCreate.PostCodeValue);
                if(_service.CreateNew(model, postCode))
                {
                    vmResult = ConvertToViewModel(model);
                    vmResult.StatusMessage = "Multi Create Complete.";
                }
                else
                {
                    vmResult.StatusMessage = "Create Failed";
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmResult.StatusMessage += Environment.NewLine + item;
                }
            }
            else
            {
                if (_service.CreateNew(model))
                {
                    vmResult = ConvertToViewModel(model);
                    vmResult.StatusMessage = "Create Complete.";
                }
                else
                {
                    vmResult.StatusMessage = "Unable to create Address Locations";
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmResult.StatusMessage += Environment.NewLine + item;
                }
            }
            return vmResult;
        }

        public AddressLocationViewModel UpdateDB(AddressLocationViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            AddressLocation model = ConvertToModel(vmUserInput);
            AddressLocationViewModel vmResult = ConvertToViewModel(model);
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
            vm.PostCodeToCreate.AvailableCities = GetAvailableCities();
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
        private Dictionary<int, string> GetAvailablePostCodes()
        {
            Dictionary<int, string> postCodes = new Dictionary<int, string>();
            var list = _service.GetAvailablePostCodes();
            if (list != null)
                foreach (var item in list)
                    postCodes.Add(item.PostCodeID, item.PostCodeID.ToString() + " (" + item.PostCodeCode + ") - " + item.PostCodeValue);
            return postCodes;
        }

        private Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _service.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }
        private Dictionary<int, string> GetAvailableCityAreas()
        {
            Dictionary<int, string> cityAreas = new Dictionary<int, string>();
            var list = _service.GetAvailableCityAreas();
            if (list != null)
                foreach (var item in list)
                    cityAreas.Add(item.CityAreaID, item.CityAreaID.ToString() + " (" + item.CityAreaCode + ") - " + item.CityAreaName);
            return cityAreas;
        }
    }
}