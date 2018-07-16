using Microsoft.AspNetCore.Mvc;

namespace FMASolutionsCore.Web.FMASolutionsWebsite.Controllers
{
    public class PoEController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
