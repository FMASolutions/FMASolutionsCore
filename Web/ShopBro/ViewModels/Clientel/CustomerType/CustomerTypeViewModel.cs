using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CustomerTypeViewModel
    {
        public CustomerTypeViewModel()
        {
            this.CustomerTypeID = 0;
            this.CustomerTypeCode = "";
            this.CustomerTypeName = "";
        }

        public Int32 CustomerTypeID { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string StatusMessage { get; set; }
    }
}