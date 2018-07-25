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
            _customerTypeService = service;
        }

        private ICustomerTypeService _customerTypeService;

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
            CustomerTypeModel model = GetModel();
            CustomerTypeViewModel vmCustomerType = new CustomerTypeViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCustomerType = model.Search(vmInput.CustomerTypeID, vmInput.CustomerTypeCode);
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
            CustomerTypeModel model = GetModel();
            return View(model.GetAllCustomerTypes());
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
            CustomerTypeModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                vm = model.Create(vm);
                if (vm.CustomerTypeID > 0)
                    return View("Display", vm);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerTypeViewModel vm)
        {
            CustomerTypeModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vm))
                {
                    vm.StatusErrorMessage = "Update processed";
                    return View("Display", vm);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vm.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vm);
        }

        private CustomerTypeModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new CustomerTypeModel(converter.Convert(), _customerTypeService);
        }
    }
}