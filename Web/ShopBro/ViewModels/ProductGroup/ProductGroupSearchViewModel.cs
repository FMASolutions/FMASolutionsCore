using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ProductGroupSearchViewModel
    {
        public ProductGroupSearchViewModel()
        {
            this.ProductGroupID = 0;
            this.ProductGroupCode = "";
        }

        public Int32 ProductGroupID { get; set; }

        [StringLength(5, ErrorMessage = "Proudct Group Code should be no more than 5 Characters")]
        public string ProductGroupCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}