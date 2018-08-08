using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class GenericSearchViewModel
    {
        public GenericSearchViewModel()
        {
            this.ID = 0;
            this.Code = "";
        }

        public Int32 ID { get; set; }
        public string Code { get; set; }
        public string SearchType {get; set;}
        public string StatusMessage { get; set; }

    }
}