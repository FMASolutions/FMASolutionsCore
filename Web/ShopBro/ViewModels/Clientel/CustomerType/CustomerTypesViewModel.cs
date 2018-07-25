using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerTypesViewModel
    {
        public CustomerTypesViewModel()
        {
            CustomerTypes = new List<CustomerTypeViewModel>();
        }
        
        public List<CustomerTypeViewModel> CustomerTypes { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}