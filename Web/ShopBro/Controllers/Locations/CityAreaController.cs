using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CityAreaController : BaseController
    {
        public CityAreaController(ICityAreaService service)
        {
            _model = new CityAreaModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ICityAreaService _service;
        private CityAreaModel _model;

        public IActionResult Index()
        {
            _model = GetNewModel();
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            vmInput.CityAreaID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CityAreaSearchViewModel vmInput)
        {
            _model = GetNewModel();
            CityAreaViewModel vmCityArea = new CityAreaViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCityArea = _model.Search(vmInput.CityAreaID, vmInput.CityAreaCode);
                if (vmCityArea.CityAreaID > 0)
                {
                    vmCityArea.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmCityArea);
                }
            }
            CityAreaSearchViewModel vmSearch = new CityAreaSearchViewModel();
            vmSearch.StatusErrorMessage = vmCityArea.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityAreaViewModel vmInput)
        {
            _model = GetNewModel();
            vmInput.AvailableCities = _model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllCityAreas());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            _model = GetNewModel();
            CityAreaViewModel vm = new CityAreaViewModel();
            vm.AvailableCities = _model.GetAvailableCities();
            if (vm.AvailableCities.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityAreaViewModel vmInput)
        {
            _model = GetNewModel();
            CityAreaViewModel vmResult = new CityAreaViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CityAreaID > 0)
                {
                    return Search(vmResult.CityAreaID);
                }
            }
            vmInput.AvailableCities = _model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityAreaViewModel vmInput)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityAreaModel GetNewModel()
        {
            return new CityAreaModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}