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

            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Search Request Received with ID = " + id.ToString());

            PostCodeSearchViewModel vmInput = new PostCodeSearchViewModel();
            vmInput.PostCodeID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(PostCodeSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Started");

            using (PostCodeModel model = GetNewModel())
            {
                PostCodeViewModel vmSearchResult = model.Search(vmInput.PostCodeID, vmInput.PostCodeCode);

                if (vmSearchResult.PostCodeID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch Item Found: " + vmSearchResult.PostCodeID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("PostCodeController.ProcessSearch No Item Found ");
                vmInput.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
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
                vmInput.AvailableCities = model.GetAvailableCities();
                return View(vmInput);
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
                PostCodeViewModel vm = new PostCodeViewModel();
                vm.AvailableCities = model.GetAvailableCities();
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
                if (vmResult.PostCodeID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Complete successfully for Value: " + vmInput.PostCodeValue);
                    return Search(vmResult.PostCodeID);
                }

                Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Create Failed, Reason: " + vmInput.StatusErrorMessage);
                vmInput.AvailableCities = model.GetAvailableCities();
                vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(PostCodeViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request Received");

            using (PostCodeModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update POST Request For: " + vmInput.PostCodeValue + " successful!");
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCities = model.GetAvailableCities();
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
                        
                    Program.loggerExtension.WriteToUserRequestLog("PostCodeController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    vmInput.AvailableCities = model.GetAvailableCities();
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private PostCodeModel GetNewModel()
        {
            return new PostCodeModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}