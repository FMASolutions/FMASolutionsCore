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
            _service = service;
        }

        private ICityService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Index Request Received");

            CitySearchViewModel vmInput = new CitySearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Search Request Received with ID = " + id.ToString());

            CitySearchViewModel vmInput = new CitySearchViewModel();
            vmInput.CityID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CitySearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Started");

            using (CityModel model = GetNewModel())
            {
                CityViewModel vmSearchResult = model.Search(vmInput.CityID, vmInput.CityCode);

                if (vmSearchResult.CityID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Item Found: " + vmSearchResult.CityID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmSearchResult);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityID.ToString());

            using (CityModel model = GetNewModel())
            {
                vmInput.AvailableCountries = model.GetAvailableCountries();
                return View(vmInput);
            }
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.DisplayAll Request Received");

            using (CityModel model = GetNewModel())
            {
                return View(model.GetAllCities());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Create GET Request Received");

            using (CityModel model = GetNewModel())
            {
                CityViewModel vm = new CityViewModel();
                vm.AvailableCountries = model.GetAvailableCountries();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Create POST Request Received");

            using (CityModel model = GetNewModel())
            {
                CityViewModel vmResult = model.Create(vmInput);
                if (vmResult.CityID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Create Complete successfully for Code: " + vmInput.CityCode);
                    return Search(vmResult.CityID);
                }

                Program.loggerExtension.WriteToUserRequestLog("CityController.Create Failed, Reason: " + vmInput.StatusMessage);
                vmInput.AvailableCountries = model.GetAvailableCountries();
                vmInput.StatusMessage = vmResult.StatusMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request Received");

            using (CityModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request For: " + vmInput.CityCode + " successful!");
                    vmInput.StatusMessage = "Update Processed";
                    vmInput.AvailableCountries = model.GetAvailableCountries();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusMessage += item + " ";

                    Program.loggerExtension.WriteToUserRequestLog("CityController.Update Failed, Reason: " + vmInput.StatusMessage);
                    vmInput.AvailableCountries = model.GetAvailableCountries();
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private CityModel GetNewModel()
        {
            return new CityModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}