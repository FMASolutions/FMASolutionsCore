using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Authorization;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class AdminPanelController : BaseController
    {
        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}