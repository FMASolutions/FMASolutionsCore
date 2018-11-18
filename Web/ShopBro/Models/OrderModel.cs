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

        private ICustomModelState _modelState;
        private IOrderService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

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
        public OrderViewModel UpdateItems(OrderViewModel newModel)
        {
            Order dbExistingOrder = _service.GetByID(newModel.OrderID);
            bool errorDetected = false;

            foreach(var item in newModel.ExistingItems)
            {
                item.OrderHeaderID = newModel.OrderID;
                if(item.OrderItemRowID == 0) //New item, this needs adding.
                {
                    if(_service.AddItemToOrder(ConvertToOrderItemModel(item)) <= 0)                    
                        errorDetected = true;
                }
            }
            foreach(var item in dbExistingOrder.OrderItems)
            {
                //Check if the item still exists in the new model, it may need deleting.
                if(!newModel.ExistingItems.Exists(e => e.OrderItemRowID == item.ID && e.OrderItemRowID != 0))
                    if(!_service.RemoveItemFromOrder(item))
                        errorDetected = true;
            }
            if(errorDetected)
                return null;
            else
                return Search(newModel.OrderID);
        }
        public int DeliverItems(int OrderHeaderID)
        {            
            DeliveryNoteViewModel vmReturn = new DeliveryNoteViewModel();
            if(OrderHeaderID > 0){
                return _service.DeliverExistingItems(OrderHeaderID);                
            }
            else
                return 0;
        }
        public DeliveryNoteViewModel GetDeliveryNote(int DeliveryNoteID)
        {
            DeliveryNoteViewModel vmReturn = new DeliveryNoteViewModel();
            if(DeliveryNoteID > 0)
                vmReturn.Title = "Great! : " + DeliveryNoteID.ToString();
            else
                vmReturn.Title = "Failed";
            return vmReturn;
        }

        private OrderViewModel ConvertToViewModel(Order model)
        {
            OrderViewModel vmReturn = new OrderViewModel();
            vmReturn.OrderID = model.Header.OrderHeaderID;            
            
            //Set up the Existing OrderItem List
            List<OrderItemViewModel> orderItems = new List<OrderItemViewModel>();
            var statusDictionary = _service.GetOrderStatusDictionary();            
            foreach(var orderItem in model.OrderItems)
            {
                OrderItemViewModel orderItemVM = new OrderItemViewModel();
                orderItemVM.ItemDescription = orderItem.OrderItemDescription;
                orderItemVM.ItemID = orderItem.ItemID;
                orderItemVM.OrderItemRowID = orderItem.OrderItemID;
                orderItemVM.Qty = orderItem.OrderItemQty;
                orderItemVM.UnitPrice = orderItem.OrderItemUnitPriceAfterDiscount;
                orderItemVM.OrderItemStatus = statusDictionary[orderItem.OrderItemStatusID];
                orderItemVM.OrderHeaderID = orderItem.OrderHeaderID;
                orderItems.Add(orderItemVM);
                if(!vmReturn.DistinctItemStatusList.Contains(orderItemVM.OrderItemStatus))
                    vmReturn.DistinctItemStatusList.Add(orderItemVM.OrderItemStatus);
            }
            vmReturn.ExistingItems = orderItems;
            vmReturn.OrderStatus = statusDictionary[model.Header.OrderStatusID];
  
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
        private OrderItem ConvertToOrderItemModel(OrderItemViewModel vm)
        {
            int i = 1;
            int itemStatus = 0;
            var itemStatusDic = _service.GetOrderStatusDictionary();
            foreach(var statusString in itemStatusDic.Values)
            {
                if(vm.OrderItemStatus == statusString)
                    itemStatus = i;
                else
                    i++;
            }
            var stock = _service.GetStockHierarchy();
            decimal originalPrice = 0.0m;
            foreach(var item in stock)
            {
                if(item.ItemID == vm.ItemID)
                    originalPrice = item.ItemUnitPrice;
            }   
            
            OrderItem returnItem = new OrderItem(
                _modelState
                ,vm.ItemID
                ,itemStatus
                ,vm.OrderHeaderID
                ,vm.ItemDescription
                ,vm.OrderItemRowID
                ,vm.Qty
                ,originalPrice
                ,vm.UnitPrice
            );

            return returnItem;
        }        
    }
}