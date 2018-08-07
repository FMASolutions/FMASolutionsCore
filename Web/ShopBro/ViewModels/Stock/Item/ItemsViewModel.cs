using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ItemsViewModel
    {
        public ItemsViewModel()
        {
            Items = new List<ItemViewModel>();
        }
        public List<ItemViewModel> Items;
        public string StatusMessage;

    }
}