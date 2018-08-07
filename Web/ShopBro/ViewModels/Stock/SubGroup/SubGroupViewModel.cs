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

        public Int32 SubGroupID { get; set; }
          
        public Int32 ProductGroupID { get; set; }

        [StringLength(5, ErrorMessage = "Sub Group Code should be no more than 5 Characters")]
        public string SubGroupCode { get; set; }

        [StringLength(100, ErrorMessage = "Sub Group Name should be no more than 100 Characters")]
        public string SubGroupName { get; set; }

        [StringLength(250, ErrorMessage = "Sub Group Description should be no more than 250 Characters")]
        public string SubGroupDescription { get; set; }

        public string StatusMessage { get; set; }
    }
}