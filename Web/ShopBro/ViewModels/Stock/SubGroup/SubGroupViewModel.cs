using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class SubGroupViewModel
    {
        public SubGroupViewModel()
        {
            AvailableProductGroups = new Dictionary<int, string>();
        }

        public Dictionary<int, string> AvailableProductGroups;

        [DisplayName("Sub Group ID ( 1 - 9,999,999 ) ")]
        public Int32 SubGroupID { get; set; }
          
        public Int32 ProductGroupID { get; set; }

        [Required(ErrorMessage = "Sub Group Code is Required")]
        [StringLength(5, ErrorMessage = "Sub Group Code should be no more than 5 Characters")]
        public string SubGroupCode { get; set; }

        [Required(ErrorMessage = "Sub Group Name is Required")]
        [StringLength(100, ErrorMessage = "Sub Group Name should be no more than 100 Characters")]
        public string SubGroupName { get; set; }

        [Required(ErrorMessage = "Sub Group Description is Required")]
        [StringLength(250, ErrorMessage = "Sub Group Description should be no more than 250 Characters")]
        public string SubGroupDescription { get; set; }

        public string StatusErrorMessage { get; set; }
    }
}