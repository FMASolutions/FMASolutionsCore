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

        [DisplayName(" ID ( 1 - 9,999,999 ) ")]
        public Int32 ProductGroupID { get; set; }

        [Required(ErrorMessage = "Product Group Code Is Required")]
        [StringLength(5, ErrorMessage = "Proudct Group Code should be no more than 5 Characters")]
        public string ProductGroupCode { get; set; }

        [Required(ErrorMessage = "Proudct Group Name is mandatory")]
        [StringLength(100, ErrorMessage = "Proudct Group Name should be no more than 100 characters")]
        public string ProductGroupName { get; set; }

        [Required(ErrorMessage = "Proudct Group Description is mandatory")]
        [StringLength(250, ErrorMessage = "Proudct Group Description should be no more than 250 characters")]
        public string ProductGroupDescription { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}