using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class AddressLocationSearchViewModel
    {
        public AddressLocationSearchViewModel()
        {
        }

        public Int32 AddressLocationID { get; set; }

        [StringLength(5, ErrorMessage = "Address Location Code should be no more than 5 Characters")]
        public string AddressLocationCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}