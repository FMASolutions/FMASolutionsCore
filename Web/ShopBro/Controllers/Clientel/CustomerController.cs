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
            _model = GetNewModel();
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            CustomerSearchViewModel vmInput = new CustomerSearchViewModel();
            vmInput.CustomerID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerSearchViewModel vmInput)
        {
            _model = GetNewModel();
            CustomerViewModel vmSubGroup = new CustomerViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSubGroup = _model.Search(vmInput.CustomerID, vmInput.CustomerCode);
                if (vmSubGroup.CustomerID > 0)
                {
                    vmSubGroup.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
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
            _model = GetNewModel();
            vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllCustomers());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
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
            _model = GetNewModel();
            CustomerViewModel vmResult = new CustomerViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.CustomerID > 0)
                {
                    return Search(vmResult.CustomerID);
                }
            }
            vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerViewModel vmInput)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCustomerTypes = _model.GetAvailableCustomerTypes();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }
        private CustomerModel GetNewModel()
        {
            return new CustomerModel(new ModelStateConverter(this).Convert(),_service);
        }
    }
}