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
            return vmResult;
        }

        public AddressLocationViewModel UpdateDB(AddressLocationViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            AddressLocation model = ConvertToModel(vmUserInput);
            AddressLocationViewModel vmResult = ConvertToViewModel(model);

            _modelState = model.ModelState;

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
            vm.AddressLine1 = model.AddressLine1;
            vm.AddressLine2 = model.AddressLine2;
            vm.CityAreaID = model.CityAreaID;
            vm.PostCode = model.PostCode;
            vm.AvailableCityAreas = GetAvailableCityAreas();
            return vm;
        }

        private AddressLocation ConvertToModel(AddressLocationViewModel vm)
        {
            AddressLocation addressLocation = new AddressLocation(_modelState
                , vm.AddressLocationID                
                , vm.CityAreaID                
                , vm.AddressLine1
                , vm.AddressLine2
                , vm.PostCode
            );
            return addressLocation;
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