using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CityAreaModel : IModel
    {
        public CityAreaModel(ICustomModelState modelState, ICityAreaService service)
        {
            _modelState = modelState;
            _cityAreaService = service;
        }
        private ICustomModelState _modelState;
        private ICityAreaService _cityAreaService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public CityAreaViewModel Search(int id = 0, string code = "")
        {
            CityArea searchResult = null;
            if (id > 0)
                searchResult = _cityAreaService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _cityAreaService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CityAreaViewModel returnVM = new CityAreaViewModel();
                returnVM.StatusErrorMessage = "No result Found";
                return returnVM;
            }
        }

        public CityAreasViewModel GetAllCityAreas()
        {
            List<CityArea> cityAreaList = _cityAreaService.GetAll();
            CityAreasViewModel vmReturn = new CityAreasViewModel();

            if (cityAreaList != null && cityAreaList.Count > 0)
            {
                foreach (CityArea item in cityAreaList)
                {
                    vmReturn.CityAreas.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No City Areas Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _cityAreaService.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }

        public CityAreaViewModel Create(CityAreaViewModel newCityArea)
        {
            CityArea cityArea = ConvertToModel(newCityArea);
            cityArea.CityAreaID = 0; //NOT SURE I NEED TRHIS???????????????
            CityAreaViewModel vmReturn = new CityAreaViewModel();
            if (_cityAreaService.CreateNew(cityArea))
                vmReturn = ConvertToViewModel(cityArea);
            else
            {
                vmReturn.StatusErrorMessage = "Unable to create City Area";
                foreach (string item in cityArea.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusErrorMessage += " " + item;
            }
            return vmReturn;
        }

        public bool UpdateDB(CityAreaViewModel updatedCityArea)
        {
            CityArea cityArea = ConvertToModel(updatedCityArea);
            if (_cityAreaService.UpdateDB(cityArea))
                return true;
            else
                _modelState = cityArea.ModelState;
            return false;
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