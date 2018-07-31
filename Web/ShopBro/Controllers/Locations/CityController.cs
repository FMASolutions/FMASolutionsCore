using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CityController : BaseController
    {
        public CityController(ICityService service)
        {
            _model = new CityModel(new ModelStateConverter(this).Convert(), service);
            _service = service;            
        }

        private ICityService _service;
        private CityModel _model;

        public IActionResult Index()
        {
            _model = GetNewModel();
            CitySearchViewModel vmInput = new CitySearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            CitySearchViewModel vmInput = new CitySearchViewModel();
            vmInput.CityID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CitySearchViewModel vmInput)
        {
            _model = GetNewModel();
            CityViewModel vmCity = new CityViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCity = _model.Search(vmInput.CityID, vmInput.CityCode);
                if (vmCity.CityID > 0)
                {
                    vmCity.AvailableCountries = _model.GetAvailableCountries();
                    return View("Display", vmCity);
                }
            }
            CitySearchViewModel vmSearch = new CitySearchViewModel();
            vmSearch.StatusErrorMessage = vmCity.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityViewModel vmInput)
        {
            _model = GetNewModel();
            vmInput.AvailableCountries = _model.GetAvailableCountries();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllCities());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            _model = GetNewModel();
            CityViewModel vm = new CityViewModel();
            vm.AvailableCountries = _model.GetAvailableCountries();
            if (vm.AvailableCountries.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityViewModel vmInput)
        {
            _model = GetNewModel();
            CityViewModel vmResult = new CityViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CityID > 0)
                {
                    return Search(vmResult.CityID);
                }
            }
            vmInput.AvailableCountries = _model.GetAvailableCountries();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityViewModel vmInput)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCountries = _model.GetAvailableCountries();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityModel GetNewModel()
        {
            return new CityModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}