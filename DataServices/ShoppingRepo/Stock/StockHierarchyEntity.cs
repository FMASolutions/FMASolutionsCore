using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class StockHierarchyEntity
    {
        public StockHierarchyEntity()
        {

        }
        public string ProductGroupName;
        public string SubGroupName;
        public string ItemName;
        public int ItemID;
        public string ItemCode;
        public int SubGroupID;
        public int ProductGroupID;
        public string ItemDescription;
        public decimal ItemUnitPrice;
        public decimal ItemUnitPriceWithMaxDiscount;
        public int ItemAvailableQty;
        public string ItemImageFilename;
        public string SubGroupCode;
        public string SubGroupDescription;
        public string ProductGroupCode;
        public string ProductGroupDescription;
    }
}