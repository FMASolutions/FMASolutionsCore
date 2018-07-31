using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CountryController : BaseController
    {
        public CountryController(ICountryService service)
        {
            _model = new CountryModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ICountryService _service;
        private CountryModel _model;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Index Request Received");
            _model = GetNewModel();
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            vmSearch.CountryID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CountrySearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Started");
            _model = GetNewModel();
            CountryViewModel vmCountry = new CountryViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCountry = _model.Search(vmInput.CountryID, vmInput.CountryCode);
                if (vmCountry.CountryID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Item Found: " + vmCountry.CountryID.ToString());
                    return View("Display", vmCountry);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch No Item Found ");
            CountrySearchViewModel searchVM = new CountrySearchViewModel();
            searchVM.StatusErrorMessage = vmCountry.StatusErrorMessage;
            return View("Search", searchVM);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.DisplayForUpdate POST Request Received For ID: " + vmInput.CountryID.ToString());
            _model = GetNewModel();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllCountries());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create GET Request Received");
            _model = GetNewModel();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                vmInput = _model.Create(vmInput);
                if (vmInput.CountryID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Complete successfully for Code: " + vmInput.CountryCode);
                    return View("Display", vmInput);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request For: " + vmInput.CountryCode + " successful!");
                    vmInput.StatusErrorMessage = "Update processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CountryModel GetNewModel()
        {
            return new CountryModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}