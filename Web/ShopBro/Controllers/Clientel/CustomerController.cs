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

            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Search Request Received with ID = " + id.ToString());

            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            vmInput.CustomerID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch Started");

            using (CustomerModel model = GetNewModel())
            {
                CustomerViewModel vmSearchResult = model.Search(vmInput.CustomerID, vmInput.CustomerCode);

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
                vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                return View(vmInput);
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
                CustomerViewModel vm = new CustomerViewModel();
                vm.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
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
                if (vmResult.CustomerID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Complete successfully for Code: " + vmInput.CustomerCode);
                    return Search(vmResult.CustomerID);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Failed, Reason: " + vmInput.StatusMessage);
                vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                vmInput.StatusMessage = vmResult.StatusMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request Received");

            using (CustomerModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request For: " + vmInput.CustomerCode + " successful!");
                    vmInput.StatusMessage = "Update Processed";
                    vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusMessage += item + " ";
                        
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update Failed, Reason: " + vmInput.StatusMessage);
                    vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private CustomerModel GetNewModel()
        {
            return new CustomerModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}