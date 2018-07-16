using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.BusinessServices.ControllerTemplate;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}