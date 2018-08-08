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

            GenericSearchViewModel vm = new GenericSearchViewModel();
            return View("Search", vm);
        }

        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vm = new GenericSearchViewModel();
            vm.ID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.ProcessSearch Started");

            using (ItemModel model = GetNewModel())
            {
                ItemViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

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
                ItemViewModel vmResult = model.Search(vmInput.ItemID,vmInput.ItemCode);
                if(vmResult.ItemID > 0)
                    return View(vmResult);
                else
                {
                    GenericSearchViewModel vm = new GenericSearchViewModel();
                    return View("Search",vm);
                }
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
                ItemViewModel vm = model.GetEmptyViewModel();
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
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Complete successfully for Code: " + vmResult.ItemCode);
                    return View("Display",vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("ItemController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ItemViewModel vmInput, IFormFile UploadFile = null)
        {
            Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request Received");

            using (ItemModel model = GetNewModel())
            {
                ItemViewModel vmResult = model.UpdateDB(vmInput,UploadFile,_hostingEnv);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update POST Request For: " + vmInput.ItemCode + " successful!");
                    return View("Display", vmResult);
                }
                    Program.loggerExtension.WriteToUserRequestLog("ItemController.Update Failed, Reason: " + vmInput.StatusMessage);
                    return View("DisplayForUpdate", vmResult);
            }
        }

        private ItemModel GetNewModel()
        {
            return new ItemModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}