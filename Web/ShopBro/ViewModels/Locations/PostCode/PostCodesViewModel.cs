using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class PostCodesViewModel
    {
        public PostCodesViewModel()
        {
            PostCodes = new List<PostCodeViewModel>();
        }
        
        public List<PostCodeViewModel> PostCodes { get; set; }
        public string StatusErrorMessage { get; set; }
    }
}