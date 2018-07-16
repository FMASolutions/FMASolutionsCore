using Microsoft.AspNetCore.Mvc;

namespace FMASolutionsCore.Web.FMASolutionsWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => this.View();

        public IActionResult About() => this.View();
    }
}