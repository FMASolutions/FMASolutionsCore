using System.Collections.Generic;

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
        public string ItemCode { get; set; }
        public int SubGroupID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemUnitPriceWithMaxDiscount { get; set; }
        public int ItemAvailableQty { get; set; }
        public int ItemReorderQtyReminder { get; set; }
        public string ItemImageFilename { get; set; }
        public string StatusMessage;
    }
}