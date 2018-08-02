using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class SubGroupSearchViewModel
    {
        public SubGroupSearchViewModel()
        {
            this.SubGroupID = 0;
            this.SubGroupCode = "";
        }

        public Int32 SubGroupID { get; set; }

        public string SubGroupCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}