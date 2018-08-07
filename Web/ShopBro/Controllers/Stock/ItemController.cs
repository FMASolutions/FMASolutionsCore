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
        }
        private IItemService _service;
        private IHostingEnvironment _hostingEnv;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Index Request Received");

            ItemSearchViewModel vm = new ItemSearchViewModel();
            return View("Search", vm);
        }

        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Search Request Received with ID = " + id.ToString());

            ItemSearchViewModel vm = new ItemSearchViewModel();
            vm.ItemID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(ItemSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch Started");

            using (ItemModel model = GetNewModel())
            {
                ItemViewModel vmSearchResult = model.Search(vmInput.ItemID, vmInput.ItemCode);

                if (vmSearchResult.ItemID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch Item Found: " + vmSearchResult.ItemID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ItemViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.DisplayForUpdate POST Request Received For ID: " + vmInput.ItemID.ToString());

            using (ItemModel model = GetNewModel())
            {
                vmInput.AvailableSubGroups = model.GetAvailableSubGroups();
                return View(vmInput);
            }
        }

        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.DisplayAll Request Received");

            using (ItemModel model = GetNewModel())
            {
                return View(model.GetAllItems());
            }
        }



        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Create GET Request Received");

            using (ItemModel model = GetNewModel())
            {
                ItemViewModel vm = new ItemViewModel();
                vm.AvailableSubGroups = model.GetAvailableSubGroups();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ItemViewModel vmInput, IFormFile uploadFile = null)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Create POST Request Received");

            using (ItemModel model = GetNewModel())
            {
                ItemViewModel vmResult = model.Create(vmInput, uploadFile, _hostingEnv);
                if (vmResult.ItemID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Complete successfully for Code: " + vmResult.ItemCode);
                    return Search(vmResult.ItemID);
                }

                Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Failed, Reason: " + vmResult.StatusMessage);
                vmInput.AvailableSubGroups = model.GetAvailableSubGroups();
                vmInput.StatusMessage = vmResult.StatusMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ItemViewModel vmInput, IFormFile UploadFile = null)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request Received");

            using (ItemModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput, UploadFile, _hostingEnv))
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request For: " + vmInput.ItemCode + " successful!");
                    vmInput.StatusMessage = "Update Processed";
                    vmInput.AvailableSubGroups = model.GetAvailableSubGroups();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)                   
                        vmInput.StatusMessage += item + " ";
                    
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update Failed, Reason: " + vmInput.StatusMessage);
                    vmInput.AvailableSubGroups = model.GetAvailableSubGroups();
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private ItemModel GetNewModel()
        {
            return new ItemModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}