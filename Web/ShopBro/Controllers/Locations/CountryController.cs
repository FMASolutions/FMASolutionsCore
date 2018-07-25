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
            _countryService = service;
        }

        private ICountryService _countryService;

        public IActionResult Index()
        {
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            return View("Search", vmSearch);
        }

        [HttpGet]
        public IActionResult Search(int id = 0)
        {
            CountrySearchViewModel vmSearch = new CountrySearchViewModel();
            vmSearch.CountryID = id;
            return ProcessSearch(vmSearch);
        }

        [HttpPost]
        public IActionResult ProcessSearch(CountrySearchViewModel vmInput)
        {
            CountryModel model = GetModel();
            CountryViewModel vmCountry = new CountryViewModel();
            if (model.ModelState.IsValid)
            {
                ModelState.Clear();
                vmCountry = model.Search(vmInput.CountryID, vmInput.CountryCode);
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
            return View(vmInput);
        }
        public IActionResult DisplayAll()
        {
            CountryModel model = GetModel();
            return View(model.GetAllCountries());
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(CountryViewModel vm)
        {
            CountryModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                vm = model.Create(vm);
                if (vm.CountryID > 0)
                    return View("Display", vm);
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(CountryViewModel vm)
        {
            CountryModel model = GetModel();
            if (model.ModelState.IsValid)
            {
                if (model.UpdateDB(vm))
                {
                    vm.StatusErrorMessage = "Update processed";
                    return View("Display", vm);
                }
                else
                    foreach (string item in model.ModelState.ErrorDictionary.Values)
                        vm.StatusErrorMessage += item + " ";
            }
            return View("DisplayForUpdate", vm);
        }

        private CountryModel GetModel()
        {
            IModelStateConverter converter = new ModelStateConverter(this);
            return new CountryModel(converter.Convert(), _countryService);
        }
    }
}