using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CityAreaSearchViewModel
    {
        public CityAreaSearchViewModel()
        {
        }

        public Int32 CityAreaID { get; set; }

        [StringLength(5, ErrorMessage = "City Area Code should be no more than 5 Characters")]
        public string CityAreaCode { get; set; }

        public string StatusMessage { get; set; }
    }
}