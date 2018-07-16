using System.ComponentModel.DataAnnotations;

namespace FMASolutionsCore.Web.FMASolutionsWebsite.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is a mandatory field")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is a mandatory field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string AuthenticationStatusMessage { get; set; }

        public int RandomIntTest { get; set; }
    }
}
