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
            _service = service;
        }

        private IPostCodeService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Index Request Received");

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Started");

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.PostCodeID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Item Found: " + vmSearchResult.PostCodeID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.DisplayForUpdate POST Request Received For ID: " + vmInput.PostCodeID.ToString());

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vmResult = model.Search(vmInput.PostCodeID);
                if(vmResult.PostCodeID > 0)
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
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.DisplayAll Request Received");

            using (PostCodeModel model = GetNewModel())
            {
                return View(model.GetAllPostCodes());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create GET Request Received");

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create POST Request Received");

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Complete successfully for Value: " + vmInput.PostCodeValue);
                    return View("Display",vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request Received");

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request For: " + vmInput.PostCodeValue + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update Failed, Reason: " + vmResult.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private PostCodeModel GetNewModel()
        {
            return new PostCodeModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}