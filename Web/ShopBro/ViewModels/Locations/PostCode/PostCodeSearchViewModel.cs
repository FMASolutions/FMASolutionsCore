using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class PostCodeSearchViewModel
    {
        public PostCodeSearchViewModel()
        {
        }

        public Int32 PostCodeID { get; set; }

        [StringLength(5, ErrorMessage = "City Area Code should be no more than 5 Characters")]
        public string PostCodeCode { get; set; }

        public string StatusMessage { get; set; }
    }
}