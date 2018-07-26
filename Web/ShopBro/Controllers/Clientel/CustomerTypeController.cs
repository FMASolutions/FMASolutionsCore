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
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            vmSearch.CustomerTypeID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerTypeSearchViewModel vmInput)
        {
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
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            return View(_model.GetAllCustomerTypes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerTypeViewModel vm)
        {
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
    }
}