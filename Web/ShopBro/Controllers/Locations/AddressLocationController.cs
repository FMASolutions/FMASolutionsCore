using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class AddressLocationController : BaseController
    {
        public AddressLocationController(IAddressLocationService service)
        {
            _service = service;
        }

        private IAddressLocationService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Index Request Received");

            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Search Request Received with ID = " + id.ToString());

            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            vmInput.AddressLocationID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(AddressLocationSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Started");

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vmSearchResult = model.Search(vmInput.AddressLocationID, vmInput.AddressLocationCode);

                if (vmSearchResult.AddressLocationID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Item Found: " + vmSearchResult.AddressLocationID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch No Item Found ");
                vmInput.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
                return View("Search", vmSearchResult);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(AddressLocationViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.DisplayForUpdate POST Request Received For ID: " + vmInput.AddressLocationID.ToString());

            using (AddressLocationModel model = GetNewModel())
            {
                vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
                vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
                return View(vmInput);
            }
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.DisplayAll Request Received");

            using (AddressLocationModel model = GetNewModel())
            {
                return View(model.GetAllAddressLocations());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create GET Request Received");

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vm = new AddressLocationViewModel();
                vm.AvailableCityAreas = model.GetAvailableCityAreas();
                vm.AvailablePostCodes = model.GetAvailablePostCodes();
                vm.postCodeToCreate.AvailableCities = model.GetAvailableCities();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(AddressLocationViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create POST Request Received");

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vmResult = model.Create(vmInput);
                if (vmResult.AddressLocationID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create Complete successfully for Code: " + vmInput.AddressLocationCode);
                    return Search(vmResult.AddressLocationID);
                }

                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
                vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
                vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
                vmInput.postCodeToCreate.AvailableCities = model.GetAvailableCities();
                vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(AddressLocationViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update POST Request Received");

            using (AddressLocationModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update POST Request For: " + vmInput.AddressLocationCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
                    vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";

                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private AddressLocationModel GetNewModel()
        {
            return new AddressLocationModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}