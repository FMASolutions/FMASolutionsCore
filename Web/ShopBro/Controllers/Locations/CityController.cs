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

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Started");

            using (CityModel model = GetNewModel())
            {
                CityViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.CityID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch Item Found: " + vmSearchResult.CityID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CityController.ProcessSearch No Item Found ");
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityID.ToString());

            using (CityModel model = GetNewModel())
            {
                CityViewModel vmResult = model.Search(vmInput.CityID);
                if (vmResult.CityID > 0)
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
                CityViewModel vm = model.GetemptyViewModel();
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
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Create Complete successfully for Code: " + vmInput.CityCode);
                    return View("Display", vmResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CityController.Create Failed, Reason: " + vmInput.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request Received");

            using (CityModel model = GetNewModel())
            {
                CityViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityController.Update POST Request For: " + vmInput.CityCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CityController.Update Failed, Reason: " + vmResult.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private CityModel GetNewModel()
        {
            return new CityModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}