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
        public AddressLocationController(IAddressLocationService service)
        {
            _model = new AddressLocationModel(new ModelStateConverter(this).Convert(), service);
            _service = service;            
        }

        private IAddressLocationService _service;
        private AddressLocationModel _model;

        public IActionResult Index()
        {
            _model = GetNewModel();
            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            return View("Search", vmInput);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            AddressLocationSearchViewModel vmInput = new AddressLocationSearchViewModel();
            vmInput.AddressLocationID = id;
            return ProcessSearch(vmInput);
        }

        [HttpPost]
        public IActionResult ProcessSearch(AddressLocationSearchViewModel vmInput)
        {
            _model = GetNewModel();
            AddressLocationViewModel vmAddressLocation = new AddressLocationViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmAddressLocation = _model.Search(vmInput.AddressLocationID, vmInput.AddressLocationCode);
                if (vmAddressLocation.AddressLocationID > 0)
                {
                    vmAddressLocation.AvailableCityAreas = _model.GetAvailableCityAreas();
                    vmAddressLocation.AvailablePostCodes = _model.GetAvailablePostCodes();
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
            _model = GetNewModel();   
            vmInput.AvailableCityAreas = _model.GetAvailableCityAreas();
            vmInput.AvailablePostCodes = _model.GetAvailablePostCodes();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllAddressLocations());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            _model = GetNewModel();
            AddressLocationViewModel vm = new AddressLocationViewModel();
            vm.AvailableCityAreas = _model.GetAvailableCityAreas();
            vm.AvailablePostCodes = _model.GetAvailablePostCodes();
            vm.postCodeToCreate.AvailableCities = _model.GetAvailableCities();
            if (vm.AvailableCityAreas.Count > 0 && vm.AvailablePostCodes.Count > 0 && vm.postCodeToCreate.AvailableCities.Count > 0)
                return View(vm);
            else
                return View("Search");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(AddressLocationViewModel vmInput)
        {         
            //_model = GetNewModel();
            AddressLocationViewModel vmResult = new AddressLocationViewModel();
            if (_model.ModelState.IsValid)
            {
                vmResult = _model.Create(vmInput);
                if (vmResult.AddressLocationID > 0)
                {
                    return Search(vmResult.AddressLocationID);
                }
            }
            vmInput.AvailableCityAreas = _model.GetAvailableCityAreas();
            vmInput.AvailablePostCodes = _model.GetAvailablePostCodes();
            vmInput.postCodeToCreate.AvailableCities = _model.GetAvailableCities();
            vmInput.StatusErrorMessage = vmResult.StatusErrorMessage;
            return View(vmInput);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(AddressLocationViewModel vmInput)
        {     
            _model = GetNewModel();       
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vmInput))
                {
                    vmInput.StatusErrorMessage = "Update Processed";
                    vmInput.AvailableCityAreas = _model.GetAvailableCityAreas();
                    vmInput.AvailablePostCodes = _model.GetAvailablePostCodes();
                    return View("Display", vmInput);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vmInput.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vmInput);
        }

        private AddressLocationModel GetNewModel()
        {
            return new AddressLocationModel(new ModelStateConverter(this).Convert(),_service);
        }
    }
}