using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;


namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class OrderHeaderController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}