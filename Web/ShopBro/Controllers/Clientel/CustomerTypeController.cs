using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CustomerTypeController : BaseController
    {
        public CustomerTypeController(ICustomerTypeService service)
        {
            _service = service;
        }

        private ICustomerTypeService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Index Request Received");

            GenericSearchViewModel vmSearch = new GenericSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmSearch = new GenericSearchViewModel();
            vmSearch.ID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch Started");

            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.CustomerTypeID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch Item Found: " + vmSearchResult.CustomerTypeID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.DisplayForUpdate POST Request Received For ID: " + vmInput.CustomerTypeID.ToString());
            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmResult = model.Search(vmInput.CustomerTypeID);
                if (vmResult.CustomerTypeID > 0)
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
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.DisplayAll Request Received");

            using (CustomerTypeModel model = GetNewModel())
            {
                return View(model.GetAllCustomerTypes());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create GET Request Received");
            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create POST Request Received");

            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Complete successfully for Code: " + vmResult.CustomerTypeCode);
                    return View("Display", vmResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request Received");

            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request For: " + vmInput.CustomerTypeCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update Failed, Reason: " + vmInput.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private CustomerTypeModel GetNewModel()
        {
            return new CustomerTypeModel(new ModelStateConverter(this).Convert(), _service);
        }

    }
}