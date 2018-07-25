using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ProductGroupsViewModel
    {
        public ProductGroupsViewModel()
        {
            ProductGroups = new List<ProductGroupViewModel>();
        }
        public List<ProductGroupViewModel> ProductGroups { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}