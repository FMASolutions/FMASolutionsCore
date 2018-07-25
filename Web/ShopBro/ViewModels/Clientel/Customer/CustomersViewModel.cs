using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomersViewModel
    {
        public CustomersViewModel()
        {
            Customers = new List<CustomerViewModel>();
        }
        public List<CustomerViewModel> Customers { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}