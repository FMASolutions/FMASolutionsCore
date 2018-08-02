using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ProductGroupViewModel
    {
        public ProductGroupViewModel()
        {
            this.ProductGroupID = 0;
            this.ProductGroupCode = "";
            this.ProductGroupName = "";
            this.ProductGroupDescription = "";
        }

        public Int32 ProductGroupID { get; set; }

        [StringLength(5, ErrorMessage = "Proudct Group Code should be no more than 5 Characters")]
        public string ProductGroupCode { get; set; }

        [StringLength(100, ErrorMessage = "Proudct Group Name should be no more than 100 characters")]
        public string ProductGroupName { get; set; }

        [StringLength(250, ErrorMessage = "Proudct Group Description should be no more than 250 characters")]
        public string ProductGroupDescription { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}