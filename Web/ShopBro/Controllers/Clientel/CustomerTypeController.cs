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
            _model = new CustomerTypeModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ICustomerTypeService _service;
        private CustomerTypeModel _model;

        public IActionResult Index()
        {
            _model = GetNewModel();
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            vmSearch.CustomerTypeID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerTypeSearchViewModel vmInput)
        {
            _model = GetNewModel();
            CustomerTypeViewModel vmCustomerType = new CustomerTypeViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCustomerType = _model.Search(vmInput.CustomerTypeID, vmInput.CustomerTypeCode);
                if (vmCustomerType.CustomerTypeID > 0)
                    return View("Display", vmCustomerType);
            }
            CustomerTypeSearchViewModel searchVM = new CustomerTypeSearchViewModel();
            searchVM.StatusErrorMessage = vmCustomerType.StatusErrorMessage;
            return View("Search", searchVM);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerTypeViewModel vmInput)
        {
            _model = GetNewModel();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllCustomerTypes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            _model = GetNewModel();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerTypeViewModel vm)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                vm = _model.Create(vm);
                if (vm.CustomerTypeID > 0)
                    return View("Display", vm);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerTypeViewModel vm)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vm))
                {
                    vm.StatusErrorMessage = "Update processed";
                    return View("Display", vm);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vm.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vm);
        }

        private CustomerTypeModel GetNewModel()
        {
            return new CustomerTypeModel(new ModelStateConverter(this).Convert(),_service);
        }
    }
}