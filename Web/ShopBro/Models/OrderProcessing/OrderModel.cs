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
        public OrdersViewModel GetAllOrders()
        {
            OrdersViewModel returnVM = new OrdersViewModel();
            var searchResults = _service.GetAllOrders();
            var customers = GetAvailableCustomers();
            var orderStatusDic = _service.GetOrderStatusDictionary();
            foreach(var order in searchResults)
            { 
                OrderViewModel current = new OrderViewModel();
                current.OrderID = order.Header.OrderHeaderID;
                current.OrderStatus = orderStatusDic.GetValueOrDefault(order.Header.OrderStatusID);
                current.DeliveryDate = order.Header.DeliveryDate;
                current.OrderDate = order.Header.OrderDate;
                current.CustomerID = order.Header.CustomerID;
                current.AvailableCustomers = customers;
                returnVM.Orders.Add(current);
            }
            return returnVM;
        }
        public OrderViewModel UpdateItems(OrderViewModel newModel)
        {
            Order dbExistingOrder = _service.GetByID(newModel.OrderID);
            Dictionary<int,string> orderStatusDic = _service.GetOrderStatusDictionary();
            List<StockHierarchyItem> stockHierarcy = _service.GetStockHierarchy();
            bool errorDetected = false;

            foreach(var item in newModel.ExistingItems)
            {
                item.OrderHeaderID = newModel.OrderID;
                if(item.OrderItemRowID == 0) //New item, this needs adding.
                {
                    if(_service.AddItemToOrder(ConvertToOrderItemModel(item,orderStatusDic,stockHierarcy)) <= 0)                    
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
        public int CreateOrder(OrderViewModel vmCreate)
        {
            OrderHeader headerModel = null;
            int returnOrderID = 0;
            vmCreate.OrderStatus = "Estimate";
            vmCreate.CustomerAddressID = vmCreate.SelectedAddressID; //CustomerAddressID here will be used to search for the addressID in customeraddressID
            headerModel = ConvertToOrderHeaderModel(vmCreate);
            if(vmCreate.UseExistingAddress)
            {        
                returnOrderID = _service.CreateOrder(headerModel);
            }
            else
            {
                AddressLocation addressLocation = new AddressLocation(
                    _modelState,0
                    ,vmCreate.NewDeliveryAddress.CityAreaID
                    , vmCreate.NewDeliveryAddress.AddressLine1
                    , vmCreate.NewDeliveryAddress.AddressLine2
                    , vmCreate.NewDeliveryAddress.PostCode
                );
                returnOrderID = _service.CreateOrder(headerModel, addressLocation);
            }
            return returnOrderID;
        }
        
        
        internal OrderViewModel GetDefaultViewModel()
        {            
            OrderViewModel vmReturn = new OrderViewModel();
            vmReturn.AvailableAddresses = GetAvailableAddresses();
            vmReturn.AvailableCustomers = GetAvailableCustomers();    
            vmReturn.NewDeliveryAddress.AvailableCityAreas = GetAvailableCityAreas();
            AppendStockHierarchyAndAvailableItems(vmReturn);
            return vmReturn;
        }
        private OrderViewModel ConvertToViewModel(Order model)
        {
            OrderViewModel vmReturn = GetDefaultViewModel();
            Dictionary<int,string> statusDictionary = _service.GetOrderStatusDictionary();                     
            vmReturn.OrderStatus = statusDictionary[model.Header.OrderStatusID];
            vmReturn.OrderID = model.Header.OrderHeaderID;            
            vmReturn.DeliveryDate = model.Header.DeliveryDate;
            vmReturn.OrderDate = model.Header.OrderDate;
            vmReturn.CustomerID = model.Header.CustomerID;
            vmReturn.CustomerAddressID = model.Header.CustomerAddressID;
            
            List<CustomerAddress> customerAddresses = _service.GetAvailableCustomerAddresses();
            var currentCustAddress = customerAddresses.Find(x => x.CustomerAddressID == model.Header.CustomerAddressID);
            var currentAddress = vmReturn.AvailableAddresses.Find(x => x.AddressLocationID == currentCustAddress.AddressLocationID);
            AddressLocationViewModel addressLocVM = new AddressLocationViewModel();
            addressLocVM.AddressLine1 = currentAddress.AddressLine1;
            addressLocVM.AddressLine2 = currentAddress.AddressLine2;
            addressLocVM.AddressLocationID = currentAddress.AddressLocationID;
            addressLocVM.AvailableCityAreas = GetAvailableCityAreas();
            addressLocVM.CityAreaID = currentAddress.CityAreaID;
            addressLocVM.PostCode = currentAddress.PostCode;
            
            vmReturn.CurrentDeliveryAddress = addressLocVM;

            var delNotes = _service.GetDeliveryNotesForOrder(model.Header.OrderHeaderID);
            var invoices = _service.GetInvoicesForOrder(model.Header.OrderHeaderID);
            
            if(delNotes != null && delNotes.Count > 0)
                foreach(var note in delNotes)
                    vmReturn.DeliveryNotesForOrder.Add(note.DeliveryNoteID);
            
            if(invoices != null && invoices.Count > 0)
                foreach(var inv in invoices)
                    vmReturn.InvoicesFOrOrder.Add(inv.Header.InvoiceHeaderID);

            //Set up the Existing OrderItem List
            List<OrderItemViewModel> orderItems = new List<OrderItemViewModel>();
            if(model.OrderItems != null && model.OrderItems.Count > 0)
            {
                foreach(var orderItem in model.OrderItems)
                {
                    OrderItemViewModel orderItemVM = ConvertToOrderItemVM(orderItem, statusDictionary);
                    orderItems.Add(orderItemVM);
                    if(!vmReturn.DistinctItemStatusList.Contains(orderItemVM.OrderItemStatus))
                        vmReturn.DistinctItemStatusList.Add(orderItemVM.OrderItemStatus);
                }
                vmReturn.ExistingItems = orderItems;
            }            
            return vmReturn;
        }
        private OrderItem ConvertToOrderItemModel(OrderItemViewModel vm, Dictionary<int, string> itemStatusDic, List<StockHierarchyItem> stockHierarcy)
        {
            int i = 1;
            int itemStatus = 0;            
            foreach(var statusString in itemStatusDic.Values)
            {
                if(vm.OrderItemStatus == statusString)
                    itemStatus = i;
                else
                    i++;
            }            
            decimal originalPrice = 0.0m;
            foreach(var item in stockHierarcy)
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
        private OrderItemViewModel ConvertToOrderItemVM(OrderItem model, Dictionary<int,string> orderStatusDic)
        {
            OrderItemViewModel orderItemVM = new OrderItemViewModel();
                orderItemVM.ItemDescription = model.OrderItemDescription;
                orderItemVM.ItemID = model.ItemID;
                orderItemVM.OrderItemRowID = model.OrderItemID;
                orderItemVM.Qty = model.OrderItemQty;
                orderItemVM.UnitPrice = model.OrderItemUnitPriceAfterDiscount;
                orderItemVM.OrderItemStatus = orderStatusDic[model.OrderItemStatusID];
                orderItemVM.OrderHeaderID = model.OrderHeaderID;
            return orderItemVM;

        }   
        private OrderHeader ConvertToOrderHeaderModel(OrderViewModel vm)
        {            
            Dictionary<int,string> statusDictionary = _service.GetOrderStatusDictionary();
            int orderStatusInt = 0;
            foreach(KeyValuePair<int,string> pair in statusDictionary)            
                if(pair.Value == vm.OrderStatus)
                    orderStatusInt = pair.Key;
            
            OrderHeader returnHeader = new OrderHeader(
                _modelState,
                vm.CustomerAddressID,
                vm.CustomerID,
                vm.DeliveryDate,
                vm.OrderDate,
                vm.OrderID,
                orderStatusInt
            );
            return returnHeader;
        }
        private Dictionary<int,string> GetAvailableCustomers()
        {
            Dictionary<int,string> availableCustomers = new Dictionary<int, string>();
            foreach(var customer in _service.GetAvailableCustomers())
                availableCustomers.Add(customer.CustomerID, customer.CustomerName);
            return availableCustomers;
        }
        private Dictionary<int,string> GetAvailableCityAreas()
        {
            List<CityArea> cityAreas = _service.GetAvailableCityAreas();
            Dictionary<int,string> returnDic = new Dictionary<int, string>();
            foreach(var cityArea in cityAreas)
                returnDic.Add(cityArea.CityAreaID, cityArea.CityAreaName);

            return returnDic;
        }
        private List<AddressLocationViewModel> GetAvailableAddresses()
        {
            List<AddressLocationViewModel> returnAddresses = new List<AddressLocationViewModel>();
            Dictionary<int,string> cityAreaDic = GetAvailableCityAreas();
            
            foreach(var address in _service.GetAvailableAddresses())
            {
                var current = new AddressLocationViewModel(){
                    AddressLine1 = address.AddressLine1
                    ,AddressLine2 = address.AddressLine2
                    ,AddressLocationID = address.AddressLocationID
                    ,CityAreaID = address.CityAreaID
                    ,PostCode = address.PostCode
                };
                current.AvailableCityAreas = cityAreaDic;
                
                returnAddresses.Add(current);
                
            }
            return returnAddresses;
        }
        private void AppendStockHierarchyAndAvailableItems(OrderViewModel vmDestination)
        {
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
            vmDestination.AvailableItems = availableItems;
            vmDestination.StockHierarchy = stockHierarchy;
        }
    }
}
