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
            OrderViewModel returnVM = null;
            var orderModel = _service.GetByID(OrderHeaderID);
            if(orderModel != null)
            {
                returnVM = ConvertToViewModel(orderModel);
                return returnVM;
            }
            else
            {
                returnVM = new OrderViewModel();
                _modelState.ErrorDictionary.Add("NoResult","No Search Result Found");
                return returnVM;
            }
        }

        private ICustomModelState _modelState;
        private IOrderService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        private OrderViewModel ConvertToViewModel(Order model)
        {
            OrderViewModel vmReturn = new OrderViewModel();
            vmReturn.OrderID = model.Header.OrderHeaderID;
            
            //Set up the Existing OrderItem List
            List<OrderItemViewModel> orderItems = new List<OrderItemViewModel>();
            foreach(var orderItem in model.OrderItems)
            {
                OrderItemViewModel orderItemVM = new OrderItemViewModel();
                orderItemVM.ItemDescription = orderItem.OrderItemDescription;
                orderItemVM.ItemID = orderItem.ItemID;
                orderItemVM.OrderItemRowID = orderItem.OrderItemID;
                orderItemVM.Qty = orderItem.OrderItemQty;
                orderItemVM.UnitPrice = orderItem.OrderItemUnitPrice;
                orderItems.Add(orderItemVM);
            }
            vmReturn.ExistingItems = orderItems;

            //Set current Order Status String Value
            foreach(var item in _service.GetOrderStatusDictionary())
                if(model.Header.OrderStatusID == item.Key)
                    vmReturn.OrderStatus = item.Value;
            
            //Set up Available Customers List
            Dictionary<int,string> availableCustomers = new Dictionary<int, string>();
            foreach(var customer in _service.GetAvailableCustomers())
                availableCustomers.Add(customer.CustomerID, customer.CustomerName);
            vmReturn.AvailableCustomers = availableCustomers;
            vmReturn.CustomerID = model.Header.CustomerID;

            
            //Set up Stock Hierarchy Object as well as Available Item List
            StockHierarchyViewModel stockHierarchy = new StockHierarchyViewModel();
            List<ItemViewModel> availableItems = new List<ItemViewModel>();
            List<StockHierarchyItem> itemDataRows = _service.GetStockHierarchy();
            foreach(var item in itemDataRows)
            {
                PGroupDetailed currentPGroupDetailed = null;
                SGroupDetailed currentSGroupDetailed = null;
                ItemViewModel currentItemViewModel = new ItemViewModel();

                if(stockHierarchy.ProductGroups.Exists(e => e.ToString() == item.ProductGroupCode))                
                    currentPGroupDetailed = stockHierarchy.ProductGroups.Find(e => e.ToString() == item.ProductGroupCode);
                else
                {
                    currentPGroupDetailed = new PGroupDetailed();
                    currentPGroupDetailed.ProductGroupCode = item.ProductGroupCode;
                    currentPGroupDetailed.ProductGroupDescription = item.ProductGroupDescription;
                    currentPGroupDetailed.ProductGroupID = item.ProductGroupID;
                    currentPGroupDetailed.ProductGroupName = item.ProductGroupName;
                    stockHierarchy.ProductGroups.Add(currentPGroupDetailed);
                }

                if(currentPGroupDetailed.AvailableSubs.Exists(e => e.ToString() == item.SubGroupCode))
                    currentSGroupDetailed = currentPGroupDetailed.AvailableSubs.Find(e => e.ToString() == item.SubGroupCode);
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
            vmReturn.AvailableItems = availableItems;
            vmReturn.StockHierarchy = stockHierarchy;
            return vmReturn;
        }
    }
}