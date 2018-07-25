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
        public PostCodeController(IPostCodeService postCodeService)
        {
            _postCodeService = postCodeService;
        }

        private IPostCodeService _postCodeService;

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
            PostCodeModel model = GetModel();
            PostCodeViewModel vmPostCode = new PostCodeViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmPostCode = model.Search(vmInput.PostCodeID, vmInput.PostCodeCode);
                if (vmPostCode.PostCodeID > 0)
                {
                    vmPostCode.AvailableCities = model.GetAvailableCities();
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
            PostCodeModel model = GetModel();
            vmInput.AvailableCities = model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            PostCodeModel model = GetModel();
            return View(model.GetAllPostCodes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            PostCodeViewModel vm = new PostCodeViewModel();
            PostCodeModel model = GetModel();
            vm.AvailableCities = model.GetAvailableCities();
            if (vm.AvailableCities.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(PostCodeViewModel vmInput)
        {
            PostCodeModel model = GetModel();
            PostCodeViewModel vmResult = new PostCodeViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.PostCodeID > 0)
                {
                    return Search(vmResult.PostCodeID);
                }
            }
            vmInput.AvailableCities = model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(PostCodeViewModel vmInput)
        {
            PostCodeModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private PostCodeModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new PostCodeModel(converter.Convert(), _postCodeService);
        }
    }
}