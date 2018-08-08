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

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Started");

            using (SubGroupModel model = GetNewModel())
            {
                SubGroupViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.SubGroupID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch Item Found: " + vmSearchResult.SubGroupID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("SubGroupController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
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
                SubGroupViewModel vmResult = model.Search(vmInput.SubGroupID);
                if(vmResult.SubGroupID > 0)
                    return View(vmResult);
                else
                {
                    GenericSearchViewModel vm = new GenericSearchViewModel();
                    return View("Search",vm);
                }
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
                SubGroupViewModel vm = model.GetEmptyViewModel();
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
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Complete successfully for Code: " + vmInput.SubGroupCode);
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(SubGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request Received");

            using (SubGroupModel model = GetNewModel())
            {
                SubGroupViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update POST Request For: " + vmInput.SubGroupCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("SubGroupController.Update Failed, Reason: " + vmResult.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private SubGroupModel GetNewModel()
        {
            return new SubGroupModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}