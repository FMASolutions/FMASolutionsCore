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
            _model = new CustomerModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ICustomerService _service;
        private CustomerModel _model;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Index Request Received");
            _model = GetNewModel();
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            vmInput.CustomerID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch Started");
            _model = GetNewModel();
            CustomerViewModel vmCustomer = new CustomerViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCustomer = _model.Search(vmInput.CustomerID, vmInput.CustomerCode);
                if (vmCustomer.CustomerID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch Item Found: " + vmCustomer.CustomerID.ToString());
                    vmCustomer.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
                    return View("Display", vmCustomer);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.ProcessSearch No Item Found ");
            CustomerSearchViewModel vmSearch = new CustomerSearchViewModel();
            vmSearch.StatusErrorMessage = vmCustomer.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.DisplayForUpdate POST Request Received For ID: " + vmInput.CustomerID.ToString());
            _model = GetNewModel();
            vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllCustomers());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create GET Request Received");
            _model = GetNewModel();
            CustomerViewModel vm = new CustomerViewModel();
            vm.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
            if (vm.AvailableCustomerTypes.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create POST Request Received");
            _model = GetNewModel();
            CustomerViewModel vmResult = new CustomerViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CustomerID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Complete successfully for Code: " + vmInput.CustomerCode);
                    return Search(vmResult.CustomerID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update POST Request For: " + vmInput.CustomerCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("CustomerController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }
        private CustomerModel GetNewModel()
        {
            return new CustomerModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}