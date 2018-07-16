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
        public ItemController(IItemService itemService, IHostingEnvironment he)
        {
            _itemService = itemService;
            _hostingEnv = he;
        }
        private IItemService _itemService;
        private IHostingEnvironment _hostingEnv;

        public IActionResult Index()
        {
            ItemSearchViewModel vm = new ItemSearchViewModel();
            return View("Search", vm);
        }

        public IActionResult Search(int id = 0)
        {
            ItemSearchViewModel vm = new ItemSearchViewModel();
            vm.ItemID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(ItemSearchViewModel vmInput)
        {
            ItemModel model = GetModel();
            ItemViewModel vmSearchResult = new ItemViewModel();

            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSearchResult = model.Search(vmInput.ItemID, vmInput.ItemCode);
                if (vmSearchResult.ItemID > 0)
                {
                    vmSearchResult.AvailableSubGroups = model.GetAvailableSubGroups();
                    return View("Display", vmSearchResult);
                }
            }
            SubGroupSearchViewModel vmSearch = new SubGroupSearchViewModel();
            vmSearch.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        public IActionResult DisplayAll()
        {
            ItemModel model = GetModel();
            return View(model.GetAllItems());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ItemViewModel vmInput)
        {
            ItemModel model = GetModel();
            ItemViewModel vm = model.Search(vmInput.ItemID, vmInput.ItemCode);
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            ItemViewModel vm = new ItemViewModel();
            ItemModel model = GetModel();
            vm.AvailableSubGroups = model.GetAvailableSubGroups();
            if (vm.AvailableSubGroups.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ItemViewModel vmInput, IFormFile uploadFile = null)
        {
            ItemModel model = GetModel();
            ItemViewModel vmResult = new ItemViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput, uploadFile, _hostingEnv);

                if (vmResult.ItemID > 0)
                {
                    return Search(vmResult.ItemID);
                }
            }
            vmInput.AvailableSubGroups = model.GetAvailableSubGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ItemViewModel vmInputs, IFormFile UploadFile = null)
        {
            ItemModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInputs, UploadFile, _hostingEnv))
                {
                    vmInputs.StatusErrorMessage = "Update Processed";
                    vmInputs.AvailableSubGroups = model.GetAvailableSubGroups();
                    return View("Display", vmInputs);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInputs.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInputs);
        }

        private ItemModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new ItemModel(converter.Convert(), _itemService);
        }
    }
}