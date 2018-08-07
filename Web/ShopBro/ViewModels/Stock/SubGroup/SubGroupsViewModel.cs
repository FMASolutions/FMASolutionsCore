using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class SubGroupsViewModel
    {
        public SubGroupsViewModel()
        {
            SubGroups = new List<SubGroupViewModel>();
        }
        public List<SubGroupViewModel> SubGroups { get; set; }
        public string StatusMessage { get; set; }
    }
}