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
            _service = service;
        }

        private ICountryService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Index Request Received");

            CountrySearchViewModel vmInput = new CountrySearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Search Request Received with ID = " + id.ToString());

            CountrySearchViewModel vmInput = new CountrySearchViewModel();
            vmInput.CountryID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CountrySearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Started");

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmSearchResult = model.Search(vmInput.CountryID, vmInput.CountryCode);

                if (vmSearchResult.CountryID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Item Found: " + vmSearchResult.CountryID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmSearchResult);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.DisplayForUpdate POST Request Received For ID: " + vmInput.CountryID.ToString());
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.DisplayAll Request Received");

            using (CountryModel model = GetNewModel())
            {
                return View(model.GetAllCountries());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create GET Request Received");
            return View();

        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create POST Request Received");

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmResult = model.Create(vmInput);
                if (vmResult.CountryID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Complete successfully for Code: " + vmInput.CountryCode);
                    return Search(vmResult.CountryID);
                }

                Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Failed, Reason: " + vmInput.StatusMessage);
                vmInput.StatusMessage = vmResult.StatusMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request Received");

            using (CountryModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request For: " + vmInput.CountryCode + " successful!");
                    vmInput.StatusMessage = "Update Processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusMessage += item + " ";

                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Update Failed, Reason: " + vmInput.StatusMessage);
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private CountryModel GetNewModel()
        {
            return new CountryModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}