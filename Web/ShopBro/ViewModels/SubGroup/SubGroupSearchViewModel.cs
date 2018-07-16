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

        [Range(1, 9999999, ErrorMessage = "ID Should be between 1 and 9,999,999")]
        [DisplayName(" ID ( 1 - 9,999,999 ) ")]
        public Int32 SubGroupID { get; set; }

        [StringLength(5, ErrorMessage = "Code should be no more than 5 Characters")]
        public string SubGroupCode { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}