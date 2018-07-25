using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class ItemViewModel
    {
        public ItemViewModel()
        {
            AvailableSubGroups = new Dictionary<int, string>();
        }
        public Dictionary<int, string> AvailableSubGroups;
        public string SubGroupDisplayString { get; set; }
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Item Code is Required")]
        [StringLength(5, ErrorMessage = "Item Code should be no more than 5 Characters")]
        public string ItemCode { get; set; }

        [Range(1, 9999999, ErrorMessage = "Sub Group ID Should be between 1 and 9,999,999")]
        [DisplayName("Sub Group ID ( 1 - 9,999,999 ) ")]
        public int SubGroupID { get; set; }

        [Required(ErrorMessage = "Item Name is Required")]
        [StringLength(100, ErrorMessage = "Item Name should be no more than 100 Characters")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Item Name is Required")]
        [StringLength(100, ErrorMessage = "Item Name should be no more than 100 Characters")]
        public string ItemDescription { get; set; }

        [Range(0, 999999999999.99)]
        public decimal ItemUnitPrice { get; set; }

        [Range(0, 999999999999.99)]
        public decimal ItemUnitPriceWithMaxDiscount { get; set; }

        [Range(0, 9999999)]
        public int ItemAvailableQty { get; set; }
        [Range(0, 9999999)]
        public int ItemReorderQtyReminder { get; set; }
        public string ItemImageFilename { get; set; }
        public string StatusErrorMessage;
    }
}