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
            _model = new ItemModel(new ModelStateConverter(this).Convert(),service);
        }
        private IItemService _service;
        private IHostingEnvironment _hostingEnv;
        private ItemModel _model;

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
            ItemViewModel vmSearchResult = new ItemViewModel();

            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSearchResult = _model.Search(vmInput.ItemID, vmInput.ItemCode);
                if (vmSearchResult.ItemID > 0)
                {
                    vmSearchResult.AvailableSubGroups = _model.GetAvailableSubGroups();
                    return View("Display", vmSearchResult);
                }
            }
            SubGroupSearchViewModel vmSearch = new SubGroupSearchViewModel();
            vmSearch.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        public IActionResult DisplayAll()
        {            
            return View(_model.GetAllItems());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ItemViewModel vmInput)
        {
            ItemViewModel vm = _model.Search(vmInput.ItemID, vmInput.ItemCode);
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
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
            ItemViewModel vmResult = new ItemViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput, uploadFile, _hostingEnv);

                if (vmResult.ItemID > 0)
                {
                    return Search(vmResult.ItemID);
                }
            }
            vmInput.AvailableSubGroups = _model.GetAvailableSubGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ItemViewModel vmInputs, IFormFile UploadFile = null)
        {            
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInputs, UploadFile, _hostingEnv))
                {
                    vmInputs.StatusErrorMessage = "Update Processed";
                    vmInputs.AvailableSubGroups = _model.GetAvailableSubGroups();
                    return View("Display", vmInputs);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInputs.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInputs);
        }
    }
}