using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ItemSearchViewModel
    {
        public ItemSearchViewModel()
        {
            this.ItemID = 0;
            this.ItemCode = "";
        }

        [DisplayName(" ID ( 1 - 9,999,999 ) ")]
        public Int32 ItemID { get; set; }

        [StringLength(5, ErrorMessage = "Code should be no more than 5 Characters")]
        public string ItemCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}