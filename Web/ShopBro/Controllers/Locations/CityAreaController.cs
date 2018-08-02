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
            _service = service;
        }

        private ICityAreaService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Index Request Received");

            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Search Request Received with ID = " + id.ToString());

            CityAreaSearchViewModel vmInput = new CityAreaSearchViewModel();
            vmInput.CityAreaID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CityAreaSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Started");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmSearchResult = model.Search(vmInput.CityAreaID, vmInput.CityAreaCode);

                if (vmSearchResult.CityAreaID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Item Found: " + vmSearchResult.CityAreaID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch No Item Found ");
                vmInput.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
                return View("Search", vmSearchResult);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityAreaID.ToString());

            using (CityAreaModel model = GetNewModel())
            {
                vmInput.AvailableCities = model.GetAvailableCities();
                return View(vmInput);
            }
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayAll Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                return View(model.GetAllCityAreas());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create GET Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vm = new CityAreaViewModel();
                vm.AvailableCities = model.GetAvailableCities();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create POST Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmResult = model.Create(vmInput);
                if (vmResult.CityAreaID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Complete successfully for Code: " + vmInput.CityAreaCode);
                    return Search(vmResult.CityAreaID);
                }

                Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
                vmInput.AvailableCities = model.GetAvailableCities();
                vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request For: " + vmInput.CityAreaCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";

                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private CityAreaModel GetNewModel()
        {
            return new CityAreaModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}