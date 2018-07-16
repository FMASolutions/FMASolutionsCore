using System.ComponentModel.DataAnnotations;

namespace FMASolutionsCore.Web.FMASolutionsWebsite.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required!")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "E-Mail address is required!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "E-Mail confirmation is required!")]
        public string EmailAddressConfirm { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required!")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public bool RememberMe { get; set; }

        public string RegistartionIssueMessage { get; set; }
    }
}