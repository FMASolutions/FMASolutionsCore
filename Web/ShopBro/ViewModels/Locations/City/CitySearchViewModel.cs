using System.ComponentModel.DataAnnotations;
using System;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class CitySearchViewModel
    {
        public CitySearchViewModel()
        {
        }

        public Int32 CityID { get; set; }

        [StringLength(5, ErrorMessage = "City Code should be no more than 5 Characters")]
        public string CityCode { get; set; }

        public string StatusMessage { get; set; }
    }
}