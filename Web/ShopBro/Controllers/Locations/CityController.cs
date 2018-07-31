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
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Index Request Received");
            _model = GetNewModel();
            CitySearchViewModel vmInput = new CitySearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            CitySearchViewModel vmInput = new CitySearchViewModel();
            vmInput.CityID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CitySearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Started");
            _model = GetNewModel();
            CityViewModel vmCity = new CityViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCity = _model.Search(vmInput.CityID, vmInput.CityCode);
                if (vmCity.CityID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Item Found: " + vmCity.CityID.ToString());
                    vmCity.AvailableCountries = _model.GetAvailableCountries();
                    return View("Display", vmCity);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch No Item Found ");
            CitySearchViewModel vmSearch = new CitySearchViewModel();
            vmSearch.StatusErrorMessage = vmCity.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityID.ToString());
            _model = GetNewModel();
            vmInput.AvailableCountries = _model.GetAvailableCountries();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllCities());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Create GET Request Received");
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
            Program.loggerExtension.WriteToUserRequestLog("CityController.Create POST Request Received");
            _model = GetNewModel();
            CityViewModel vmResult = new CityViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CityID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Create Complete successfully for Code: " + vmInput.CityCode);
                    return Search(vmResult.CityID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CityController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            vmInput.AvailableCountries = _model.GetAvailableCountries();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request For: " + vmInput.CityCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCountries = _model.GetAvailableCountries();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityModel GetNewModel()
        {
            return new CityModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}