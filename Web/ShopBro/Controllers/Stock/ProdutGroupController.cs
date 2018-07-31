using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class ProductGroupController : BaseController
    {
        public ProductGroupController(IProductGroupService service)
        {
            _service = service;
            _model = new ProductGroupModel(new ModelStateConverter(this).Convert(), _service);
        }

        private IProductGroupService _service;
        private ProductGroupModel _model;
        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Index Request Received");
            _model = GetNewModel();
            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            vmSearch.ProductGroupID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(ProductGroupSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Started");
            _model = GetNewModel();
            ProductGroupViewModel vmProductGroup = new ProductGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmProductGroup = _model.Search(vmInput.ProductGroupID, vmInput.ProductGroupCode);
                if (vmProductGroup.ProductGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Item Found: " + vmProductGroup.ProductGroupID.ToString());
                    return View("Display", vmProductGroup);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch No Item Found ");
            ProductGroupSearchViewModel searchVM = new ProductGroupSearchViewModel();
            searchVM.StatusErrorMessage = vmProductGroup.StatusErrorMessage;
            return View("Search", searchVM);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllProductGroups());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.DisplayForUpdate POST Request Received For ID: " + vmInput.ProductGroupID.ToString());
            _model = GetNewModel();
            return View(vmInput);
        }


        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create GET Request Received");
            _model = GetNewModel();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                vmInput = _model.Create(vmInput);
                if (vmInput.ProductGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Complete successfully for Code: " + vmInput.ProductGroupCode);
                    return View("Display", vmInput);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Failed, Reason: " + vmInput.StatusErrorMessage);

            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request For: " + vmInput.ProductGroupCode + " successful!");
                    vmInput.StatusErrorMessage = "Update processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private ProductGroupModel GetNewModel()
        {
            return new ProductGroupModel(new ModelStateConverter(this).Convert(), _service);
        }

    }
}