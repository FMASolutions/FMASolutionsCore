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

            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Search Request Received with ID = " + id.ToString());

            CustomerTypeSearchViewModel vmSearch = new CustomerTypeSearchViewModel();
            vmSearch.CustomerTypeID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CustomerTypeSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.ProcessSearch Started");

            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmSearchResult = model.Search(vmInput.CustomerTypeID, vmInput.CustomerTypeCode);

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
            return View(vmInput);
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
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create POST Request Received");

            using (CustomerTypeModel model = GetNewModel())
            {
                CustomerTypeViewModel vmResult = model.Create(vmInput);
                if (vmResult.CustomerTypeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Complete successfully for Code: " + vmResult.CustomerTypeCode);
                    return Search(vmResult.CustomerTypeID);
                }

                Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Create Failed, Reason: " + vmResult.StatusMessage);
                vmInput.StatusMessage = vmResult.StatusMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CustomerTypeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request Received");

            using (CustomerTypeModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update POST Request For: " + vmInput.CustomerTypeCode + " successful!");
                    vmInput.StatusMessage = "Update processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusMessage += item + " ";
                        
                    Program.loggerExtension.WriteToUserRequestLog("CustomerTypeController.Update Failed, Reason: " + vmInput.StatusMessage);
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private CustomerTypeModel GetNewModel()
        {
            return new CustomerTypeModel(new ModelStateConverter(this).Convert(), _service);
        }

    }
}