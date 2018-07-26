using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class PostCodeController : BaseController
    {
        public PostCodeController(IPostCodeService service)
        {
            _model = new PostCodeModel(new ModelStateConverter(this).Convert(),service);
            _service = service;
        }

        private IPostCodeService _service;
        private PostCodeModel _model;

        public IActionResult Index()
        {
            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            vmInput.PostCodeID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(PostCodeSearchViewModel vmInput)
        {
            PostCodeViewModel vmPostCode = new PostCodeViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmPostCode = _model.Search(vmInput.PostCodeID, vmInput.PostCodeCode);
                if (vmPostCode.PostCodeID > 0)
                {
                    vmPostCode.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmPostCode);
                }
            }
            PostCodeSearchViewModel vmSearch = new PostCodeSearchViewModel();
            vmSearch.StatusErrorMessage = vmPostCode.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(PostCodeViewModel vmInput)
        {
            vmInput.AvailableCities = _model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            return View(_model.GetAllPostCodes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            PostCodeViewModel vm = new PostCodeViewModel();
            vm.AvailableCities = _model.GetAvailableCities();
            if (vm.AvailableCities.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(PostCodeViewModel vmInput)
        {
            PostCodeViewModel vmResult = new PostCodeViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.PostCodeID > 0)
                {
                    return Search(vmResult.PostCodeID);
                }
            }
            vmInput.AvailableCities = _model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(PostCodeViewModel vmInput)
        {
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }
    }
}