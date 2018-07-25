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
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private ICustomerService _customerService;

        public IActionResult Index()
        {
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            vmInput.CustomerID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerSearchViewModel vmInput)
        {
            CustomerModel model = GetModel();
            CustomerViewModel vmSubGroup = new CustomerViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSubGroup = model.Search(vmInput.CustomerID, vmInput.CustomerCode);
                if (vmSubGroup.CustomerID > 0)
                {
                    vmSubGroup.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                    return View("Display", vmSubGroup);
                }
            }
            CustomerSearchViewModel vmSearch = new CustomerSearchViewModel();
            vmSearch.StatusErrorMessage = vmSubGroup.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerViewModel vmInput)
        {
            CustomerModel model = GetModel();
            vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            CustomerModel model = GetModel();
            return View(model.GetAllCustomers());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            CustomerViewModel vm = new CustomerViewModel();
            CustomerModel model = GetModel();
            vm.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
            if (vm.AvailableCustomerTypes.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerViewModel vmInput)
        {
            CustomerModel model = GetModel();
            CustomerViewModel vmResult = new CustomerViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.CustomerID > 0)
                {
                    return Search(vmResult.CustomerID);
                }
            }
            vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerViewModel vmInput)
        {
            CustomerModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCustomerTypes = model.GetAvailableCustomerTypes();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CustomerModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new CustomerModel(converter.Convert(), _customerService);
        }
    }
}