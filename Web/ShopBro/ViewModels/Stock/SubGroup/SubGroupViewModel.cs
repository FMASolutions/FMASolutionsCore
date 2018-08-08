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
        public string SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public string SubGroupDescription { get; set; }
        public string StatusMessage { get; set; }
    }
}