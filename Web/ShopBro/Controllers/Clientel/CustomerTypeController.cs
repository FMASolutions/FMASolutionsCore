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
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Index Request Received");
            _model = GetNewModel();
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            vmSearch.CustomerTypeID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerTypeSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch Started");
            _model = GetNewModel();
            CustomerTypeViewModel vmCustomerType = new CustomerTypeViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCustomerType = _model.Search(vmInput.CustomerTypeID, vmInput.CustomerTypeCode);
                if (vmCustomerType.CustomerTypeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch Item Found: " + vmCustomerType.CustomerTypeID.ToString());
                    return View("Display", vmCustomerType);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch No Item Found ");
            CustomerTypeSearchViewModel searchVM = new CustomerTypeSearchViewModel();
            searchVM.StatusErrorMessage = vmCustomerType.StatusErrorMessage;
            return View("Search", searchVM);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.DisplayForUpdate POST Request Received For ID: " + vmInput.CustomerTypeID.ToString());
            _model = GetNewModel();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllCustomerTypes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create GET Request Received");
            _model = GetNewModel();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                vmInput = _model.Create(vmInput);
                if (vmInput.CustomerTypeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Complete successfully for Code: " + vmInput.CustomerTypeCode);
                    return View("Display", vmInput);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request For: " + vmInput.CustomerTypeCode + " successful!");
                    vmInput.StatusErrorMessage = "Update processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private CustomerTypeModel GetNewModel()
        {
            return new CustomerTypeModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}