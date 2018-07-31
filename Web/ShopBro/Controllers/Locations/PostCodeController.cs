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
            _model = new PostCodeModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private IPostCodeService _service;
        private PostCodeModel _model;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Index Request Received");
            _model = GetNewModel();
            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            vmInput.PostCodeID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(PostCodeSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Started");
            _model = GetNewModel();
            PostCodeViewModel vmPostCode = new PostCodeViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmPostCode = _model.Search(vmInput.PostCodeID, vmInput.PostCodeCode);
                if (vmPostCode.PostCodeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Item Found: " + vmPostCode.PostCodeID.ToString());
                    vmPostCode.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmPostCode);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch No Item Found ");
            PostCodeSearchViewModel vmSearch = new PostCodeSearchViewModel();
            vmSearch.StatusErrorMessage = vmPostCode.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.DisplayForUpdate POST Request Received For ID: " + vmInput.PostCodeID.ToString());
            _model = GetNewModel();
            vmInput.AvailableCities = _model.GetAvailableCities();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllPostCodes());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create GET Request Received");
            _model = GetNewModel();
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
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create POST Request Received");
            _model = GetNewModel();
            PostCodeViewModel vmResult = new PostCodeViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.PostCodeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Complete successfully for Code: " + vmInput.PostCodeCode);
                    return Search(vmResult.PostCodeID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            vmInput.AvailableCities = _model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request For: " + vmInput.PostCodeValue + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = _model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private PostCodeModel GetNewModel()
        {
            return new PostCodeModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}