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
        public CityAreaController(ICityAreaService cityAreaService)
        {
            _cityAreaService = cityAreaService;
        }

        private ICityAreaService _cityAreaService;

        public IActionResult Index()
        {
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            vmInput.CityAreaID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CityAreaSearchViewModel vmInput)
        {
            CityAreaModel model = GetModel();
            CityAreaViewModel vmCityArea = new CityAreaViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCityArea = model.Search(vmInput.CityAreaID, vmInput.CityAreaCode);
                if (vmCityArea.CityAreaID > 0)
                {
                    vmCityArea.AvailableCities = model.GetAvailableCities();
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
            CityAreaModel model = GetModel();
            vmInput.AvailableCities = model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            CityAreaModel model = GetModel();
            return View(model.GetAllCityAreas());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            CityAreaViewModel vm = new CityAreaViewModel();
            CityAreaModel model = GetModel();
            vm.AvailableCities = model.GetAvailableCities();
            if (vm.AvailableCities.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityAreaViewModel vmInput)
        {
            CityAreaModel model = GetModel();
            CityAreaViewModel vmResult = new CityAreaViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.CityAreaID > 0)
                {
                    return Search(vmResult.CityAreaID);
                }
            }
            vmInput.AvailableCities = model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityAreaViewModel vmInput)
        {
            CityAreaModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityAreaModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new CityAreaModel(converter.Convert(), _cityAreaService);
        }
    }
}