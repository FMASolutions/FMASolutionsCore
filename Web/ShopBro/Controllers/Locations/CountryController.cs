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

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Started");

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.CountryID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch Item Found: " + vmSearchResult.CountryID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CountryController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.DisplayForUpdate POST Request Received For ID: " + vmInput.CountryID.ToString());

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmResult = model.Search(vmInput.CountryID);
                if (vmResult.CountryID > 0)
                    return View(vmResult);
                else
                {
                    GenericSearchViewModel vm = new GenericSearchViewModel();
                    return View("Search", vm);
                }
            }
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
            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Create POST Request Received");

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Complete successfully for Code: " + vmInput.CountryCode);
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CountryController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CountryViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request Received");

            using (CountryModel model = GetNewModel())
            {
                CountryViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CountryController.Update POST Request For: " + vmInput.CountryCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CountryController.Update Failed, Reason: " + vmResult.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private CountryModel GetNewModel()
        {
            return new CountryModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}