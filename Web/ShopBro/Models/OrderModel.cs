using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class OrderModel : IModel, IDisposable
    {
        public OrderModel(ICustomModelState modelState, IOrderService orderService)
        {
            _modelState = modelState;
            _service = orderService;
        }
        public void Dispose()
        {           
            _service.Dispose();
        }

        public OrderViewModel Search(int OrderHeaderID)
        {
            OrderViewModel vm = new OrderViewModel();
            List<OrderItemViewModel> currentItems = new List<OrderItemViewModel>();
            List<ItemViewModel> availableItems = new List<ItemViewModel>();
            Dictionary<int,string> availableCustomers = new Dictionary<int, string>();

            OrderItemViewModel item1 = new OrderItemViewModel();
            OrderItemViewModel item2 = new OrderItemViewModel();
            item1.ItemID = 31;
            item2.ItemID = 83;
            item1.Qty = 5;
            item2.Qty = 3;
            item1.ItemDescription = "Jet Wash";
            item2.ItemDescription = "Shower Head";
            item1.UnitPrice = 3.50m;
            item2.UnitPrice = 4.75m;
            item1.OrderItemRowID = 786;
            item2.OrderItemRowID = 321;
            currentItems.Add(item1);
            currentItems.Add(item2);


            availableCustomers.Add(1,"FMA Solutions LTD!");
            availableCustomers.Add(2,"Some Other Company");

            ItemViewModel availItem1 = new ItemViewModel();
            availItem1.ItemAvailableQty = 50;
            availItem1.ItemCode = "Code1";
            availItem1.ItemDescription = "First Item Description";
            availItem1.ItemID = 26;
            availItem1.ItemName = "First Item Name";
            availItem1.ItemUnitPrice = 2.99m;
            availItem1.ItemUnitPriceWithMaxDiscount = 1.99m;
            availItem1.SubGroupID = 1;


            availableItems.Add(availItem1);
            
            
            vm.CustomerID = 1;
            vm.ExistingItems = currentItems;
            vm.AvailableItems = availableItems;
            vm.AvailableCustomers = availableCustomers;
            vm.OrderID = OrderHeaderID;
            vm.OrderStatus = "Estimate";

            StockHierarchyViewModel hierarchy = new StockHierarchyViewModel();
            var tableData = _service.GetStockHierarchy();
            
            foreach(var item in tableData)
            {
                PGroupDetailed currentPGroupDetailed = null;
                SGroupDetailed currentSGroupDetailed = null;
                ItemViewModel currentItemViewModel = new ItemViewModel();

                if(hierarchy.ProductGroups.Exists(e => e.ToString() == item.ProductGroupCode))                
                    currentPGroupDetailed = hierarchy.ProductGroups.Find(e => e.ToString() == item.ProductGroupCode);
                else
                {
                    currentPGroupDetailed = new PGroupDetailed();
                    currentPGroupDetailed.ProductGroupCode = item.ProductGroupCode;
                    currentPGroupDetailed.ProductGroupDescription = item.ProductGroupDescription;
                    currentPGroupDetailed.ProductGroupID = item.ProductGroupID;
                    currentPGroupDetailed.ProductGroupName = item.ProductGroupName;
                    hierarchy.ProductGroups.Add(currentPGroupDetailed);
                }
                
                if(currentPGroupDetailed.AvailableSubs.Exists(e => e.ToString() == item.SubGroupCode))
                {
                    currentSGroupDetailed = currentPGroupDetailed.AvailableSubs.Find(e => e.ToString() == item.SubGroupCode);
                }
                else
                {
                    //Add new SubGroup to currentPGroup if it's not in the list already.
                    currentSGroupDetailed = new SGroupDetailed();
                    currentSGroupDetailed.SubGroupCode = item.SubGroupCode;
                    currentSGroupDetailed.SubGroupDescription = item.SubGroupDescription;
                    currentSGroupDetailed.SubGroupID = item.SubGroupID;
                    currentSGroupDetailed.SubGroupName = item.SubGroupName;
                    currentPGroupDetailed.AvailableSubs.Add(currentSGroupDetailed);
                }
                currentItemViewModel.ItemAvailableQty = item.ItemAvailableQty;
                currentItemViewModel.ItemCode = item.ItemCode;
                currentItemViewModel.ItemDescription = item.ItemDescription;
                currentItemViewModel.ItemID = item.ItemID;
                currentItemViewModel.ItemImageFilename = item.ItemImageFilename;
                currentItemViewModel.ItemName = item.ItemName;
                currentItemViewModel.ItemUnitPrice = item.ItemUnitPrice;
                currentItemViewModel.ItemUnitPriceWithMaxDiscount = item.ItemUnitPriceWithMaxDiscount;
                currentItemViewModel.SubGroupID = item.SubGroupID;
                
                currentSGroupDetailed.AvailableItems.Items.Add(currentItemViewModel);
            }
            
            vm.StockHierarchy = hierarchy;
            return vm;
        }

        private ICustomModelState _modelState;
        private IOrderService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        

    }
}