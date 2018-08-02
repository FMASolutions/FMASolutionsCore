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
            _service = service;
        }

        private ISubGroupService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Index Request Received");

            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Search Request Received with ID = " + id.ToString());

            SubGroupSearchViewModel vmInput = new SubGroupSearchViewModel();
            vmInput.SubGroupID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(SubGroupSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Started");

            using (SubGroupModel model = GetNewModel())
            {
                SubGroupViewModel vmSearchResult = model.Search(vmInput.SubGroupID, vmInput.SubGroupCode);

                if (vmSearchResult.SubGroupID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Item Found: " + vmSearchResult.SubGroupID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch No Item Found ");
                vmInput.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.DisplayForUpdate POST Request Received For ID: " + vmInput.SubGroupID.ToString());

            using (SubGroupModel model = GetNewModel())
            {
                vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
                return View(vmInput);
            }
        }
        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.DisplayAll Request Received");

            using (SubGroupModel model = GetNewModel())
            {
                return View(model.GetAllSubGroups());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create GET Request Received");

            using (SubGroupModel model = GetNewModel())
            {
                SubGroupViewModel vm = new SubGroupViewModel();
                vm.AvailableProductGroups = model.GetAvailableProductGroups();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create POST Request Received");

            using (SubGroupModel model = GetNewModel())
            {
                SubGroupViewModel vmResult = model.Create(vmInput);
                if (vmResult.SubGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Complete successfully for Code: " + vmInput.SubGroupCode);
                    return Search(vmResult.SubGroupID);
                }

                Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
                vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
                vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request Received");

            using (SubGroupModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request For: " + vmInput.SubGroupCode + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
                        
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    vmInput.AvailableProductGroups = model.GetAvailableProductGroups();
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private SubGroupModel GetNewModel()
        {
            return new SubGroupModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}