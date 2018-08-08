using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CityAreaController : BaseController
    {
        public CityAreaController(ICityAreaService service)
        {
            _service = service;
        }

        private ICityAreaService _service;

        public IActionResult Index()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Index Request Received");

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Search Request Received with ID = " + id.ToString());

            GenericSearchViewModel vmInput = new GenericSearchViewModel();
            vmInput.ID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Started");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmSearchResult = model.Search(vmInput.ID, vmInput.Code);

                if (vmSearchResult.CityAreaID > 0)
                {
                    ModelState.Clear();
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch Item Found: " + vmSearchResult.CityAreaID.ToString());
                    return View("Display", vmSearchResult);
                }
                Program.loggerExtension.WriteToUserRequestLog("CityAreaController.ProcessSearch No Item Found ");
                vmInput.StatusMessage = vmSearchResult.StatusMessage;
                return View("Search", vmInput);
            }
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayForUpdate POST Request Received For ID: " + vmInput.CityAreaID.ToString());

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmResult = model.Search(vmInput.CityAreaID);
                if(vmResult.CityAreaID > 0)
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
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.DisplayAll Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                return View(model.GetAllCityAreas());
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create GET Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vm = model.GetEmptyViewModel();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create POST Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmResult = model.Create(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Complete successfully for Code: " + vmInput.CityAreaCode);
                    return View("Display",vmResult);
                }

                Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Create Failed, Reason: " + vmInput.StatusMessage);
                return View(vmResult);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CityAreaViewModel vmInput)
        {
            Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request Received");

            using (CityAreaModel model = GetNewModel())
            {
                CityAreaViewModel vmResult = model.UpdateDB(vmInput);
                if (model.ModelState.IsValid)
                {
                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update POST Request For: " + vmInput.CityAreaCode + " successful!");
                    return View("Display", vmResult);
                }
                else
                {
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusMessage += item + " ";

                    Program.loggerExtension.WriteToUserRequestLog("CityAreaController.Update Failed, Reason: " + vmResult.StatusMessage);
                    return View("DisplayForUpdate", vmResult);
                }
            }
        }

        private CityAreaModel GetNewModel()
        {
            return new CityAreaModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}