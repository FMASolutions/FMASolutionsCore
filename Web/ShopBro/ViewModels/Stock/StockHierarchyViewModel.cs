using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class PGroupDetailed
    {
            public int ProductGroupID {get;set;}
            public string ProductGroupName {get;set;}
            public string ProductGroupDescription {get;set;}
            public string ProductGroupCode {get;set;}
            public List<SGroupDetailed> AvailableSubs = new List<SGroupDetailed>();

            public override string ToString()
            {
                return ProductGroupCode;
            }
    }
    public class SGroupDetailed
    {
        public int SubGroupID {get; set;}
        public string SubGroupName {get;set;}
        public string SubGroupDescription{get;set;}
        public string SubGroupCode{get;set;}
        public ItemsViewModel AvailableItems = new ItemsViewModel();

        public override string ToString()
        {
            return SubGroupCode;
        }

    }
    public class StockHierarchyViewModel
    {
                       
        public StockHierarchyViewModel()
        {
            ProductGroups = new List<PGroupDetailed>();
        }

        public List<PGroupDetailed> ProductGroups;
    }
}