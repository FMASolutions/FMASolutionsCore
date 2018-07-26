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
            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            vmInput.SubGroupID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(SubGroupSearchViewModel vmInput)
        {            
            SubGroupViewModel vmSubGroup = new SubGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSubGroup = _model.Search(vmInput.SubGroupID, vmInput.SubGroupCode);
                if (vmSubGroup.SubGroupID > 0)
                {                    
                    return View("Display", vmSubGroup);
                }
            }
            SubGroupSearchViewModel vmSearch = new SubGroupSearchViewModel();
            vmSearch.StatusErrorMessage = vmSubGroup.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(SubGroupViewModel vmInput)
        {
            vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {            
            return View(_model.GetAllSubGroups());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
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
            SubGroupViewModel vmResult = new SubGroupViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.SubGroupID > 0)
                {
                    return Search(vmResult.SubGroupID);
                }
            }
            vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(SubGroupViewModel vmInput)
        {            
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableProductGroups = _model.GetAvailableProductGroups();
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