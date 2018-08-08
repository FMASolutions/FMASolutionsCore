using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class ProductGroupController : BaseController
    {
        public ProductGroupController(IProductGroupService service)
        {
            _service = service;
        }

        private IProductGroupService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Index Request Received");

            GenericSearchViewModel vmSearch = new GenericSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmSearch = new GenericSearchViewModel();
            vmSearch.ID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Started");

            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.ProductGroupID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Item Found: " + vmSearchResult.ProductGroupID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.DisplayForUpdate POST Request Received For ID: " + vmInput.ProductGroupID.ToString());
            
            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmResult = model.Search(vmInput.ProductGroupID);
                if (vmResult.ProductGroupID > 0)
                    return View(vmResult);
                else
                {
                    GenericSearchViewModel vm = new GenericSearchViewModel();
                    return View("Search", vm);
                }
            }
        }

        public IActionResult DisplayAll()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.DisplayAll Request Received");

            using (ProductGroupModel model = GetNewModel())
            {
                return View(model.GetAllProductGroups());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create GET Request Received");
            
            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create POST Request Received");

            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Complete successfully for Code: " + vmResult.ProductGroupCode);
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Failed, Reason: " + vmResult.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request Received");

            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request For: " + vmInput.ProductGroupCode + " successful!");
                    return View("Display", vmResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update Failed, Reason: " + vmInput.StatusMessage);
                return View("DisplayForUpdate", vmResult);
            }
        }

        private ProductGroupModel GetNewModel()
        {
            return new ProductGroupModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}