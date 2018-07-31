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
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Index Request Received");
            _model = GetNewModel();
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            vmInput.CityAreaID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CityAreaSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Started");
            _model = GetNewModel();
            CityAreaViewModel vmCityArea = new CityAreaViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCityArea = _model.Search(vmInput.CityAreaID, vmInput.CityAreaCode);
                if (vmCityArea.CityAreaID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Item Found: " + vmCityArea.CityAreaID.ToString());
                    vmCityArea.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmCityArea);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch No Item Found ");
            CityAreaSearchViewModel vmSearch = new CityAreaSearchViewModel();
            vmSearch.StatusErrorMessage = vmCityArea.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityAreaID.ToString());
            _model = GetNewModel();
            vmInput.AvailableCities = _model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllCityAreas());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create GET Request Received");
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
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create POST Request Received");
            _model = GetNewModel();
            CityAreaViewModel vmResult = new CityAreaViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CityAreaID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Complete successfully for Code: " + vmInput.CityAreaCode);
                    return Search(vmResult.CityAreaID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            vmInput.AvailableCities = _model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request For: " + vmInput.CityAreaCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CityAreaModel GetNewModel()
        {
            return new CityAreaModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}