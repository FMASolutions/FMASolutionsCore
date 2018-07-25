using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class AddressLocationController : BaseController
    {
        public AddressLocationController(IAddressLocationService addressLocationService)
        {
            _addressLocationService = addressLocationService;
        }

        private IAddressLocationService _addressLocationService;

        public IActionResult Index()
        {
            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            vmInput.AddressLocationID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(AddressLocationSearchViewModel vmInput)
        {
            AddressLocationModel model = GetModel();
            AddressLocationViewModel vmAddressLocation = new AddressLocationViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmAddressLocation = model.Search(vmInput.AddressLocationID, vmInput.AddressLocationCode);
                if (vmAddressLocation.AddressLocationID > 0)
                {
                    vmAddressLocation.AvailableCityAreas = model.GetAvailableCityAreas();
                    vmAddressLocation.AvailablePostCodes = model.GetAvailablePostCodes();
                    return View("Display", vmAddressLocation);
                }
            }
            AddressLocationSearchViewModel vmSearch = new AddressLocationSearchViewModel();
            vmSearch.StatusErrorMessage = vmAddressLocation.StatusErrorMessage;
            return View("Search", vmSearch);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(AddressLocationViewModel vmInput)
        {
            AddressLocationModel model = GetModel();
            vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
            vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            AddressLocationModel model = GetModel();
            return View(model.GetAllAddressLocations());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            AddressLocationViewModel vm = new AddressLocationViewModel();
            AddressLocationModel model = GetModel();
            vm.AvailableCityAreas = model.GetAvailableCityAreas();
            vm.AvailablePostCodes = model.GetAvailablePostCodes();
            if (vm.AvailableCityAreas.Count > 0 && vm.AvailablePostCodes.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(AddressLocationViewModel vmInput)
        {
            AddressLocationModel model = GetModel();
            AddressLocationViewModel vmResult = new AddressLocationViewModel();
            if (model.ModelState.IsValid)
            {
                vmResult = model.Create(vmInput);
                if (vmResult.AddressLocationID > 0)
                {
                    return Search(vmResult.AddressLocationID);
                }
            }
            vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
            vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(AddressLocationViewModel vmInput)
        {
            AddressLocationModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCityAreas = model.GetAvailableCityAreas();
                    vmInput.AvailablePostCodes = model.GetAvailablePostCodes();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private AddressLocationModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new AddressLocationModel(converter.Convert(), _addressLocationService);
        }
    }
}