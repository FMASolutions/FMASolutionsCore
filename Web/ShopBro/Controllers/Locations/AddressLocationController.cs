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

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Started");

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.AddressLocationID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch Item Found: " + vmSearchResult.AddressLocationID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(AddressLocationViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.DisplayForUpdate POST Request Received For ID: " + vmInput.AddressLocationID.ToString());

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vmResult = model.Search(vmInput.AddressLocationID);
                if (vmResult.AddressLocationID > 0)
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
                AddressLocationViewModel vm = model.GetEmptyViewModel();
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
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create Complete successfully for ID: " + vmResult.AddressLocationID.ToString());
                    return View("Display", vmResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Create Failed, Reason: " + vmInput.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(AddressLocationViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update POST Request Received");

            using (AddressLocationModel model = GetNewModel())
            {
                AddressLocationViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update POST Request For ID: " + vmInput.AddressLocationID.ToString() + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("AddressLocationController.Update Failed, Reason: " + vmInput.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private AddressLocationModel GetNewModel()
        {
            return new AddressLocationModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}