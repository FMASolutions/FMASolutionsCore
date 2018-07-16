using FMASolutionsCore.Web.FMASolutionsWebsite.Models;
using FMASolutionsCore.Web.FMASolutionsWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FMASolutionsCore.Web.FMASolutionsWebsite.Controllers
{
    public class UserAccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vmUser)
        {
            UserAccountModel model;
            if (TryValidateModel((object)vmUser))
            {
                model = new UserAccountModel(vmUser.Username, vmUser.Password);
                UserProfileViewModel vmProfile = new UserProfileViewModel();
                if (model.HasValidCredentials())
                {
                    vmProfile.AuthMessage = model.AuthMessage;
                    vmUser.AuthenticationStatusMessage = model.AuthMessage;
                    return View("Profile", vmProfile);
                }
                vmProfile.AuthMessage = model.AuthMessage;
                vmUser.AuthenticationStatusMessage = model.AuthMessage;
            }
            return View("Login", vmUser);
        }

        public IActionResult Logout(string Username)
        {
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