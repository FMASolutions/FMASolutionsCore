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
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        private ICityService _cityService;

        public IActionResult Index()
        {
            CitySearchViewModel vmInput = new CitySearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            CitySearchViewModel vmInput = new CitySearchViewModel();
            vmInput.CityID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CitySearchViewModel vmInput)
        {
            CityModel model = GetModel();
            CityViewModel vmCity = new CityViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCity = model.Search(vmInput.CityID, vmInput.CityCode);
                if (vmCity.CityID > 0)
                {
                    vmCity.AvailableCountries = model.GetAvailableCountries();
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
            CityModel model = GetModel();
            vmInput.AvailableCountries = model.GetAvailableCountries();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            CityModel model = GetModel();
            return View(model.GetAllCities());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            CityViewModel vm = new CityViewModel();
            CityModel model = GetModel();
            vm.AvailableCountries = model.GetAvailableCountries();
            if (vm.AvailableCountries.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityViewModel vmInput)
        {
            CityModel model = GetModel();
            CityViewModel vmResult = new CityViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.CityID > 0)
                {
                    return Search(vmResult.CityID);
                }
            }
            vmInput.AvailableCountries = model.GetAvailableCountries();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityViewModel vmInput)
        {
            CityModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCountries = model.GetAvailableCountries();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new CityModel(converter.Convert(), _cityService);
        }
    }
}