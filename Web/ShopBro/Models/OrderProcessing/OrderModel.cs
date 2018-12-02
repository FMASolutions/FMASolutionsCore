using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class OrderModel : IModel, IDisposable
    {
        public OrderModel(ICustomModelState modelState, IOrderService orderService, IOrderItemService orderItemService)
        {
            _modelState = modelState;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }
        public void Dispose()
        {
            _orderService.Dispose();
        }


        private ICustomModelState _modelState;
        private IOrderService _orderService;
        private IOrderItemService _orderItemService;


        public ICustomModelState ModelState { get { return _modelState; } }
        public DisplayOrderViewModel Search(int OrderHeaderID)
        {
            DisplayOrderViewModel returnVM = null;
            var orderModel = _orderService.GetOrderHeader(OrderHeaderID);
            var items = _orderItemService.GetOrderItemsForOrder(OrderHeaderID);
            if(orderModel != null && items != null)
            {                
                returnVM = ConvertToDisplayViewModel(orderModel, items);
            }
            return returnVM;
        }
        public DisplayAllOrdersViewModel GetAllOrders()
        {
            DisplayAllOrdersViewModel returnVM = new DisplayAllOrdersViewModel();
            var searchResults = _orderService.GetAllOrders();
            if(searchResults != null)
                foreach(var order in searchResults)
                    returnVM.Orders.Add(order);
            else
                returnVM.StatusMessage = "No Orders Found";
            return returnVM;
        }
        public DisplayOrderViewModel UpdateItems(AmendOrderItemsViewModel newModel)
        {
            //DO MORE WORK ON THIS DEFO!!!!!!!!!! CLEAN UP !!!!!!!!!
            var dbExistingOrderItems = _orderItemService.GetOrderItemsForOrder(newModel.HeaderDetail.OrderID);
            bool errorDetected = false;
            foreach(var item in newModel.ItemDetails)
            {
                if(item.OrderItemID == 0) //New item, this needs adding.
                {
                    int orderItemID = _orderItemService.AddItemToOrder(ConvertDetailedItemDTOToCreateItemDTO(item));
                    if(orderItemID == 0)                    
                        errorDetected = true;
                }
            }
            foreach(var item in dbExistingOrderItems)
            {
                //Check if the item still exists in the new model, it may need deleting.
                if(!newModel.ItemDetails.Exists(e => e.OrderItemID == item.OrderItemID && e.OrderItemID != 0))
                    if(!_orderItemService.RemoveItemFromOrder(item.OrderItemID))
                        errorDetected = true;
            }
            if(errorDetected)
                return null;
            else
                return Search(newModel.HeaderDetail.OrderID);
        }
        public int CreateOrder(CreateOrderViewModel vmCreate)
        {
            OrderHeaderCreationDTO creationDTO = new OrderHeaderCreationDTO();
            creationDTO.DeliveryDate = vmCreate.OrderDeliveryDueDate;
            creationDTO.OrderDate = vmCreate.OrderDate;
            int createdOrderID = 0;

            if(vmCreate.UseExistingAddress)
            {
                createdOrderID = _orderService.CreateOrderWithExistingAddress(creationDTO, vmCreate.SelectedAddressID, vmCreate.SelectedCustomerID);
            }   
            else
            {
                AddressLocation addLoc = new AddressLocation(_modelState, vmCreate.SelectedAddressID
                    , vmCreate.NewAddressLocationVM.CityAreaID
                    , vmCreate.NewAddressLocationVM.AddressLine1
                    ,vmCreate.NewAddressLocationVM.AddressLine2
                    ,vmCreate.NewAddressLocationVM.PostCode
                );
                createdOrderID = _orderService.CreateOrderWithNewAddress(creationDTO, addLoc, vmCreate.SelectedCustomerID);
            }         

            return createdOrderID;
        }
        public AmendOrderItemsViewModel GetAmendOrderItemsViewModel(int orderID)
        {
            AmendOrderItemsViewModel returnVM = null;
            var searchHeader = _orderService.GetOrderHeaderDetailed(orderID);
            var searchItems = _orderItemService.GetOrderItemsDetailed(orderID);
            if(searchItems != null)
            {
                returnVM = new AmendOrderItemsViewModel();
                foreach(var result in searchItems)
                    returnVM.ItemDetails.Add(result);

                AppendStockHierarchyAndAvailableItems(returnVM);
            }
            return returnVM;
        }       
        
        public CreateOrderViewModel GetEmptyCreateModel()
        {
            CreateOrderViewModel emptyModel = new CreateOrderViewModel();
            AddressLocationViewModel newAddressVM = new AddressLocationViewModel();
            
            var cityAreas = _orderService.GetAvailableCityAreas();
            var addresses = _orderService.GetAvailableAddresses();
            var customers = _orderService.GetAvailableCustomers();
            
            foreach(var address in addresses)
                emptyModel.AvailableAddresses.Add(address.AddressLocationID, address.AddressLine1 + " " + address.AddressLine2 + ", " + address.PostCode);
            foreach(var cityArea in cityAreas)
                newAddressVM.AvailableCityAreas.Add(cityArea.CityAreaID, cityArea.CityAreaName);
            foreach(var customer in customers)
                emptyModel.AvailableCustomers.Add(customer.CustomerID, customer.CustomerName);

            emptyModel.NewAddressLocationVM = newAddressVM;
            emptyModel.OrderDeliveryDueDate = DateTime.Now;
            emptyModel.OrderDate = DateTime.Now.AddDays(3);
            return emptyModel;
        }
        

        private DisplayOrderViewModel ConvertToDisplayViewModel(OrderHeaderDTO header, IEnumerable<OrderItemDTO> items)
        {
            DisplayOrderViewModel returnVM = new DisplayOrderViewModel();
            returnVM.OrderHeader = header;

            if(items != null)
                foreach(var item in items)
                    returnVM.OrderItems.Add(item);

            var delNoteSearchResult = _orderService.GetDeliveryNotesForOrder(header.OrderID);
            if(delNoteSearchResult != null)
                foreach(var item in delNoteSearchResult)
                    returnVM.DeliveryNotesForOrder.Add(item);
            
            var invSearchResult = _orderService.GetInvoicesForOrder(header.OrderID);
            if(invSearchResult != null)
                foreach(var item in invSearchResult)
                    returnVM.InvoicesForOrder.Add(item);

            return returnVM;
        }
        private OrderItemCreationDTO ConvertDetailedItemDTOToCreateItemDTO(OrderItemDetailedDTO detailedDTO)
        {
            OrderItemCreationDTO creationDTO = new OrderItemCreationDTO();
            creationDTO.ItemID = detailedDTO.ItemID;
            creationDTO.OrderHeaderID = detailedDTO.OrderID;
            creationDTO.OrderItemDescription = detailedDTO.OrderItemDescription;
            creationDTO.OrderItemQty = detailedDTO.OrderItemQty;
            creationDTO.OrderItemStatusID = 1;
            creationDTO.OrderItemUnitPrice = detailedDTO.OrderItemUnitPrice;
            creationDTO.OrderItemUnitPriceAfterDiscount = detailedDTO.OrderItemUnitPriceAfterDiscount;
            return creationDTO;
        }
        private void AppendStockHierarchyAndAvailableItems(AmendOrderItemsViewModel vmDestination)
        {
            //Set up Stock Hierarchy Object as well as Available Item List
            StockHierarchyViewModel stockHierarchy = new StockHierarchyViewModel();
            List<ItemViewModel> availableItems = new List<ItemViewModel>();
            List<StockHierarchyItem> itemDataRows = _orderService.GetStockHierarchy();
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
