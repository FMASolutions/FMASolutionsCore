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

            var orderModel = _service.GetByID(OrderHeaderID);
            foreach(var item in orderModel.OrderItems)
            {
                OrderItemViewModel i = new OrderItemViewModel();
                i.ItemDescription = item.OrderItemDescription;
                i.ItemID = item.ItemID;
                i.OrderItemRowID = item.OrderItemID;
                i.Qty = item.OrderItemQty;
                i.UnitPrice = item.OrderItemUnitPrice;
                currentItems.Add(i);
            }

            availableCustomers.Add(1,"FMA Solutions LTD!");
            availableCustomers.Add(2,"Some Other Company");

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
                availableItems.Add(currentItemViewModel);
            }
            
            vm.CustomerID = 1;
            vm.ExistingItems = currentItems;
            vm.AvailableItems = availableItems;
            vm.AvailableCustomers = availableCustomers;
            vm.OrderID = OrderHeaderID;
            vm.OrderStatus = "Estimate";
            
            vm.StockHierarchy = hierarchy;
            return vm;
        }

        private ICustomModelState _modelState;
        private IOrderService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        

    }
}