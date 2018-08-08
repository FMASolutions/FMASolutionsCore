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
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductGroupDescription { get; set; }
        public string StatusMessage { get; set; }
    }
}