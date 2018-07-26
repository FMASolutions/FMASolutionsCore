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
            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            vmSearch.ProductGroupID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(ProductGroupSearchViewModel vmInput)
        {            
            ProductGroupViewModel vmProductGroup = new ProductGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmProductGroup = _model.Search(vmInput.ProductGroupID, vmInput.ProductGroupCode);
                if (vmProductGroup.ProductGroupID > 0)
                    return View("Display", vmProductGroup);
            }
            ProductGroupSearchViewModel searchVM = new ProductGroupSearchViewModel();
            searchVM.StatusErrorMessage = vmProductGroup.StatusErrorMessage;
            return View("Search", searchVM);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ProductGroupViewModel vmInput)
        {
            return View(vmInput);

        }
        public IActionResult DisplayAll()
        {
            return View(_model.GetAllProductGroups());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ProductGroupViewModel vm)
        {            
            if (_model.ModelState.IsValid)
            {
                vm = _model.Create(vm);
                if (vm.ProductGroupID > 0)
                    return View("Display", vm);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ProductGroupViewModel vm)
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