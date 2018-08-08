using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CityAreaModel : IModel, IDisposable
    {
        public CityAreaModel(ICustomModelState modelState, ICityAreaService service)
        {
            _modelState = modelState;
            _service = service;
        }
        public void Dispose()
        {
            _service.Dispose();
        }
        private ICustomModelState _modelState;
        private ICityAreaService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public CityAreaViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new CityArea(_modelState));
        }
        public CityAreaViewModel Search(int id = 0, string code = "")
        {
            CityArea searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CityAreaViewModel returnVM = new CityAreaViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }

        public CityAreasViewModel GetAllCityAreas()
        {
            List<CityArea> cityAreaList = _service.GetAll();
            CityAreasViewModel vmReturn = new CityAreasViewModel();

            if (cityAreaList != null && cityAreaList.Count > 0)
            {
                foreach (CityArea item in cityAreaList)
                {
                    vmReturn.CityAreas.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No City Areas Found";
            return vmReturn;
        }

        public CityAreaViewModel Create(CityAreaViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            CityArea model = ConvertToModel(vmUserInput);
            CityAreaViewModel vmResult = ConvertToViewModel(model);

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

        public CityAreaViewModel UpdateDB(CityAreaViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            CityArea model = ConvertToModel(vmUserInput);
            CityAreaViewModel vmResult = ConvertToViewModel(model);
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

        private Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _service.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }
        private CityAreaViewModel ConvertToViewModel(CityArea model)
        {
            CityAreaViewModel vm = new CityAreaViewModel();
            vm.CityID = model.CityID;
            vm.CityAreaID = model.CityAreaID;
            vm.CityAreaCode = model.CityAreaCode;
            vm.CityAreaName = model.CityAreaName;            
            vm.AvailableCities = GetAvailableCities();
            return vm;
        }

        private CityArea ConvertToModel(CityAreaViewModel vm)
        {
            CityArea cityArea = new CityArea(_modelState
                , vm.CityAreaID
                , vm.CityAreaCode
                , vm.CityID
                , vm.CityAreaName
            );
            return cityArea;
        }
    }
}