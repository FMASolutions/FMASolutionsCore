using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        private ICustomerService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Index Request Received");

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch Started");

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.CustomerID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch Item Found: " + vmSearchResult.CustomerID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.DisplayForUpdate POST Request Received For ID: " + vmInput.CustomerID.ToString());

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vmResult = model.Search(vmInput.CustomerID);
                if (vmResult.CustomerID > 0)
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
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.DisplayAll Request Received");

            using (CustomerModel model = GetNewModel())
            {
                return View(model.GetAllCustomers());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create GET Request Received");

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create POST Request Received");

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Complete successfully for Code: " + vmInput.CustomerCode);
                    return View("Display", vmResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request Received");

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request For: " + vmInput.CustomerCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update Failed, Reason: " + vmInput.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private CustomerModel GetNewModel()
        {
            return new CustomerModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}