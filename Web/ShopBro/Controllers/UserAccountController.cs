using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ControllerTemplate;
using System.Security.Claims; //ClaimTypes
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class UserAccountController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            string returnURL = this.Request.Query["ReturnURL"];
            if (returnURL != null && returnURL.Length > 0)
                TempData["ReturnURL"] = returnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vmUser)
        {
            if (string.IsNullOrEmpty(vmUser.Username) || string.IsNullOrEmpty(vmUser.Password))
            {
                vmUser.StatusErrorMessage = "username or password cannot be empty";
                return View("Login", vmUser);
            }

            if (vmUser.Username == "faisal" && vmUser.Password == "pakistan786")
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, vmUser.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                UserProfileViewModel vmProfile = new UserProfileViewModel();
                vmProfile.AuthMessage = "Login success";
                string redirectURL = "";
                if (TempData["ReturnURL"] != null)
                    redirectURL = TempData["ReturnURL"].ToString();
                if (redirectURL.Length > 0)
                    return Redirect(redirectURL);
                return Redirect("/Home");
            }
            else
            {
                return View("Login", vmUser);
            }
        }

        public async Task<IActionResult> Logout(string Username)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel vmRegistration)
        {
            vmRegistration.RegistartionIssueMessage = "Registration is currently closed at this time!";
            return View(vmRegistration);
        }
    }
}