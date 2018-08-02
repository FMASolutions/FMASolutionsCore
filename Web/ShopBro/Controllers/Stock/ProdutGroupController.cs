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

            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Search Request Received with ID = " + id.ToString());

            ProductGroupSearchViewModel vmSearch = new ProductGroupSearchViewModel();
            vmSearch.ProductGroupID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(ProductGroupSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Started");

            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmSearchResult = model.Search(vmInput.ProductGroupID, vmInput.ProductGroupCode);

                if (vmSearchResult.ProductGroupID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch Item Found: " + vmSearchResult.ProductGroupID.ToString());
                    return View("Display", vmSearchResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.ProcessSearch No Item Found ");
                vmInput.StatusErrorMessage = vmSearchResult.StatusErrorMessage;
                return View("Search", vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.DisplayForUpdate POST Request Received For ID: " + vmInput.ProductGroupID.ToString());
            return View(vmInput);
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
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create POST Request Received");

            using (ProductGroupModel model = GetNewModel())
            {
                ProductGroupViewModel vmResult = model.Create(vmInput);
                if (vmResult.ProductGroupID > 0)
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Complete successfully for Code: " + vmResult.ProductGroupCode);
                    return Search(vmResult.ProductGroupID);
                }

                Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Create Failed, Reason: " + vmResult.StatusErrorMessage);
                vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
                return View(vmInput);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(ProductGroupViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request Received");

            using (ProductGroupModel model = GetNewModel())
            {
                if (model.UpdateDB(vmInput))
                {
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update POST Request For: " + vmInput.ProductGroupCode + " successful!");
                    vmInput.StatusErrorMessage = "Update processed";
                    return View("Display", vmInput);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
                        
                    Program.loggerExtension.WriteToUserRequestLog("ProductGroupController.Update Failed, Reason: " + vmInput.StatusErrorMessage);
                    return View("DisplayForUpdate", vmInput);
                }
            }
        }

        private ProductGroupModel GetNewModel()
        {
            return new ProductGroupModel(new ModelStateConverter(this).Convert(), _service);
        }

    }
}