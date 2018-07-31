using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class CountryController : BaseController
    {
        public CountryController(ICountryService service)
        {
            _model = new CountryModel(new ModelStateConverter(this).Convert(), service);
            _service = service;
        }

        private ICountryService _service;
        private CountryModel _model;

        public IActionResult Index()
        {
            _model = GetNewModel();
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            _model = GetNewModel();
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            vmSearch.CountryID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CountrySearchViewModel vmInput)
        {
            _model = GetNewModel();
            CountryViewModel vmCountry = new CountryViewModel();
            if (_model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCountry = _model.Search(vmInput.CountryID, vmInput.CountryCode);
                if (vmCountry.CountryID > 0)
                    return View("Display", vmCountry);
            }
            CountrySearchViewModel searchVM = new CountrySearchViewModel();
            searchVM.StatusErrorMessage = vmCountry.StatusErrorMessage;
            return View("Search", searchVM);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult DisplayForUpdate(CountryViewModel vmInput)
        {
            _model = GetNewModel();
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            _model = GetNewModel();
            return View(_model.GetAllCountries());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            _model = GetNewModel();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CountryViewModel vm)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                vm = _model.Create(vm);
                if (vm.CountryID > 0)
                    return View("Display", vm);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CountryViewModel vm)
        {
            _model = GetNewModel();
            if (_model.ModelState.IsValid)
            {
                if (_model.UpdateDB(vm))
                {
                    vm.StatusErrorMessage = "Update processed";
                    return View("Display", vm);
                }
                else
                    foreach (string item in _model.ModelState.ErrorDictionary.Values)
                        vm.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vm);
        }
        
        private CountryModel GetNewModel()
        {
            return new CountryModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}