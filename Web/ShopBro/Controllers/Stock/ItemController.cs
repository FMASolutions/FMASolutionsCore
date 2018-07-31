using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class ItemController : BaseController
    {
        public ItemController(IItemService service, IHostingEnvironment he)
        {
            _service = service;
            _hostingEnv = he;
            _model = new ItemModel(new ModelStateConverter(this).Convert(), service);
        }
        private IItemService _service;
        private IHostingEnvironment _hostingEnv;
        private ItemModel _model;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Index Request Received");
            _model = GetNewModel();
            ItemSearchViewModel vm = new ItemSearchViewModel();
            return View("Search", vm);
        }

        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            ItemSearchViewModel vm = new ItemSearchViewModel();
            vm.ItemID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(ItemSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch Started");
            _model = GetNewModel();
            ItemViewModel vmSearchResult = new ItemViewModel();

            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSearchResult = _model.Search(vmInput.ItemID, vmInput.ItemCode);
                if (vmSearchResult.ItemID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch Item Found: " + vmSearchResult.ItemID.ToString());
                    vmSearchResult.AvailableSubGroups = _model.GetAvailableSubGroups();
                    return View("Display", vmSearchResult);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch No Item Found ");
            ItemSearchViewModel vmSearch = new ItemSearchViewModel();
            vmSearch.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllItems());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ItemViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.DisplayForUpdate POST Request Received For ID: " + vmInput.ItemID.ToString());
            _model = GetNewModel();
            ItemViewModel vm = _model.Search(vmInput.ItemID, vmInput.ItemCode);
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Create GET Request Received");
            _model = GetNewModel();
            ItemViewModel vm = new ItemViewModel();
            vm.AvailableSubGroups = _model.GetAvailableSubGroups();
            if (vm.AvailableSubGroups.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ItemViewModel vmInput, IFormFile uploadFile = null)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Create POST Request Received");
            _model = GetNewModel();
            ItemViewModel vmResult = new ItemViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput, uploadFile, _hostingEnv);

                if (vmResult.ItemID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Complete successfully for Code: " + vmResult.ItemCode);
                    return Search(vmResult.ItemID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Failed, Reason: " + vmResult.StatusErrorMessage);
            vmInput.AvailableSubGroups = _model.GetAvailableSubGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;

            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ItemViewModel vmInput, IFormFile UploadFile = null)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput, UploadFile, _hostingEnv))
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request For: " + vmInput.ItemCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableSubGroups = _model.GetAvailableSubGroups();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private ItemModel GetNewModel()
        {
            return new ItemModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}