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

        [StringLength(5, ErrorMessage = "Item Code should be no more than 5 Characters")]
        public string ItemCode { get; set; }

        public int SubGroupID { get; set; }

        [StringLength(100, ErrorMessage = "Item Name should be no more than 100 Characters")]
        public string ItemName { get; set; }

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
        public string StatusMessage;
    }
}