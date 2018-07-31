using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class SubGroupController : BaseController
    {
        public SubGroupController(ISubGroupService service)
        {
            _model = new SubGroupModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ISubGroupService _service;
        private SubGroupModel _model;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Index Request Received");
            _model = GetNewModel();
            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Search Request Received with ID = " + id.ToString());
            _model = GetNewModel();
            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            vmInput.SubGroupID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(SubGroupSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Started");
            _model = GetNewModel();
            SubGroupViewModel vmSubGroup = new SubGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSubGroup = _model.Search(vmInput.SubGroupID, vmInput.SubGroupCode);
                if (vmSubGroup.SubGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Item Found: " + vmSubGroup.SubGroupID.ToString());
                    return View("Display", vmSubGroup);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch No Item Found ");
            SubGroupSearchViewModel vmSearch = new SubGroupSearchViewModel();
            vmSearch.StatusErrorMessage = vmSubGroup.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.DisplayForUpdate POST Request Received For ID: " + vmInput.SubGroupID.ToString());
            _model = GetNewModel();
            vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.DisplayAll Request Received");
            _model = GetNewModel();
            return View(_model.GetAllSubGroups());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create GET Request Received");
            _model = GetNewModel();
            SubGroupViewModel vm = new SubGroupViewModel();
            vm.AvailableProductGroups = _model.GetAvailableProductGroups();
            if (vm.AvailableProductGroups.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create POST Request Received");
            _model = GetNewModel();
            SubGroupViewModel vmResult = new SubGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.SubGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Complete successfully for Code: " + vmInput.SubGroupCode);
                    return Search(vmResult.SubGroupID);
                }
            }
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
            vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request Received");
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request For: " + vmInput.SubGroupCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                    {
                        vmInput.StatusErrorMessage += item + " ";
                    }
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                }
            }
            return View("DisplayForUpdate", vmInput);
        }

        private SubGroupModel GetNewModel()
        {
            return new SubGroupModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}