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
        public SubGroupController(ISubGroupService subGroupService)
        {
            _subGroupService = subGroupService;
        }

        private ISubGroupService _subGroupService;

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
            SubGroupModel model = GetModel();
            SubGroupViewModel vmSubGroup = new SubGroupViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmSubGroup = model.Search(vmInput.SubGroupID, vmInput.SubGroupCode);
                if (vmSubGroup.SubGroupID > 0)
                {
                    vmSubGroup.AvailableProductGroups = model.GetAvailableProductGroups();
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
            SubGroupModel model = GetModel();
            vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            SubGroupModel model = GetModel();
            return View(model.GetAllSubGroups());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            SubGroupViewModel vm = new SubGroupViewModel();
            SubGroupModel model = GetModel();
            vm.AvailableProductGroups = model.GetAvailableProductGroups();
            if (vm.AvailableProductGroups.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(SubGroupViewModel vmInput)
        {
            SubGroupModel model = GetModel();
            SubGroupViewModel vmResult = new SubGroupViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.SubGroupID > 0)
                {
                    return Search(vmResult.SubGroupID);
                }
            }
            vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(SubGroupViewModel vmInput)
        {
            SubGroupModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private SubGroupModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new SubGroupModel(converter.Convert(), _subGroupService);
        }
    }
}