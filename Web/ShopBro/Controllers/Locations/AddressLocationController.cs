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
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Started");
                AddressLocationViewModel vmAddressLocation = new AddressLocationViewModel();
                if (model.ModelState.IsValid)
                {
                    ModelState.Clear();
                    vmAddressLocation = model.Search(vmInput.AddressLocationID, vmInput.AddressLocationCode);
                    if (vmAddressLocation.AddressLocationID > 0)
                    {
                        Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Item Found: " + vmAddressLocation.AddressLocationID.ToString());
                        vmAddressLocation.AvailableCityAreas = model.GetAvailableCityAreas();
                        vmAddressLocation.AvailablePostCodes = model.GetAvailablePostCodes();
                        return View("Display", vmAddressLocation);
                    }
                }
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch No Item Found ");
                AddressLocationSearchViewModel vmSearch = new AddressLocationSearchViewModel();
                vmSearch.StatusErrorMessage = vmAddressLocation.StatusErrorMessage;
                return View("Search", vmSearch);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(AddressLocationViewModel vmInput)
        {
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.DisplayForUpdate POST Request Received For ID: " + vmInput.AddressLocationID.ToString());
                vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
                vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
                return View(vmInput);
            }
        }
        public IActionResult DisplayAll()
        {
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.DisplayAll Request Received");
                return View(model.GetAllAddressLocations());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create GET Request Received");
                AddressLocationViewModel vm = new AddressLocationViewModel();
                vm.AvailableCityAreas = model.GetAvailableCityAreas();
                vm.AvailablePostCodes = model.GetAvailablePostCodes();
                vm.postCodeToCreate.AvailableCities = model.GetAvailableCities();
                if (vm.AvailableCityAreas.Count > 0 && vm.AvailablePostCodes.Count > 0 && vm.postCodeToCreate.AvailableCities.Count > 0)
                    return View(vm);
                else
                    return View("Search");
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(AddressLocationViewModel vmInput)
        {
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create POST Request Received");
                AddressLocationViewModel vmResult = new AddressLocationViewModel();
                if (model.ModelState.IsValid)
                {
                    vmResult = model.Create(vmInput);
                    if (vmResult.AddressLocationID > 0)
                    {
                        Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create Complete successfully for Code: " + vmInput.AddressLocationCode);
                        return Search(vmResult.AddressLocationID);
                    }
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
            using (AddressLocationModel model = GetNewModel())
            {
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update POST Request Received");
                if (model.ModelState.IsValid)
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
                        {
                            vmInput.StatusErrorMessage += item + " ";
                        }
                        Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    }
                }
                return View("DisplayForUpdate", vmInput);
            }
        }

        private AddressLocationModel GetNewModel()
        {
            return new AddressLocationModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}