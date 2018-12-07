using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class OrderService : IOrderService
    {
        public OrderService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
        }
        internal OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _uow.Dispose();
            }
        }
        private bool _disposing = false;
        private IUnitOfWork _uow;

        
        public OrderHeaderDTO GetOrderHeader(int orderHeaderID)
        {
            return _uow.OrderHeaderRepo.GetOrderHeader(orderHeaderID);
        }
        public int CreateOrderWithNewAddress(OrderHeaderCreationDTO orderHeader, AddressLocation newAddress, int customerID)
        {
            IAddressLocationService _addressService = new AddressLocationService(_uow);
            ICustomerAddressService _customerAddressService = new CustomerAddressService(_uow);
            ICustomerService _customerService = new CustomerService(_uow);

            _addressService.CreateNew(newAddress);
            int addressLocationID = _uow.AddressLocationRepo.GetMostRecent();
            _customerAddressService.CreateNew(new CustomerAddress(new CustomModelState(), 0, customerID, addressLocationID, false , "AUTO DESC"));
            int customerAddressID = _uow.CustomerAddressRepo.GetCustomerAddressIDByCustomerAndAddress(customerID, addressLocationID);

            return CreateOrder(customerAddressID, customerID, orderHeader.OrderDate, orderHeader.DeliveryDate, 1);
        }
        public int CreateOrderWithExistingAddress(OrderHeaderCreationDTO orderHeader,int addressID, int customerID)
        {
            IAddressLocationService _addressService = new AddressLocationService(_uow);
            ICustomerAddressService _customerAddressService = new CustomerAddressService(_uow);
            int customerAddressID = _uow.CustomerAddressRepo.GetCustomerAddressIDByCustomerAndAddress(customerID, addressID);
            if(customerAddressID == 0)
                if(_customerAddressService.CreateNew(new CustomerAddress(new CustomModelState(), 0, customerID, addressID, false, "AUTO DESC")))
                    customerAddressID = _uow.CustomerAddressRepo.GetCustomerAddressIDByCustomerAndAddress(customerID, addressID);
            
            return CreateOrder(customerAddressID, customerID, orderHeader.OrderDate, orderHeader.DeliveryDate, 1);
        }
        private int CreateOrder(int customerAddressID, int customerID, DateTime orderDate, DateTime deliveryDate, int statusID)
        {
            OrderHeaderEntity headerToCreate = new OrderHeaderEntity();
            headerToCreate.CustomerAddressID = customerAddressID;
            headerToCreate.CustomerID = customerID;
            headerToCreate.DeliveryDate = deliveryDate;
            headerToCreate.OrderDate = orderDate;
            headerToCreate.OrderStatusID = statusID; //Always Create at estimate stage...
            int returnID = 0;
            if(_uow.OrderHeaderRepo.Create(headerToCreate))
            {
                returnID = _uow.OrderHeaderRepo.GetLatestOrder().OrderHeaderID;
                _uow.SaveChanges();
            }
            return returnID;
        }
        public IEnumerable<OrderPreviewDTO> GetAllOrders()
        {
            return _uow.OrderHeaderRepo.GetAllOrderPreviews();
        }    
        public IEnumerable<OrderPreviewDTO> GetOrdersByCustomerCode(string customerCode)
        {
            return _uow.OrderHeaderRepo.GetOrdersByCustomerCode(customerCode);
        }
        public OrderHeaderDetailedDTO GetOrderHeaderDetailed(int orderHeaderID)
        {
            return _uow.OrderHeaderRepo.GetOrderHeaderDetailed(orderHeaderID);
        }
        public Dictionary<int, string> GetOrderStatusDictionary()
        {
            Dictionary<int, string> returnDic = new Dictionary<int, string>();
            
            foreach(var item in _uow.OrderStatusRepo.GetAll())
                returnDic.Add(item.OrderStatusID, item.OrderStatusValue);

            return returnDic;
        }
        
        public int AddItemToOrder(OrderItemCreationDTO item)
        {
            bool success = false;
            int orderItemID = -1; //Error Value
            item.OrderItemStatusID = 1; //Estimate as new item
            OrderItemEntity entity = new OrderItemEntity(0, item.OrderHeaderID, item.ItemID, item.OrderItemStatusID
                ,item.OrderItemUnitPrice, item.OrderItemUnitPriceAfterDiscount, item.OrderItemQty, item.OrderItemDescription);
            success = _uow.OrderItemRepo.Create(entity);
            if(success)
            {   
                _uow.SaveChanges();                     
                return _uow.OrderItemRepo.GetLatestOrderItemByOrder(item.OrderHeaderID);
            }
            
            return orderItemID;           
        }
        public bool RemoveItemFromOrder(int orderItemID)
        {
            OrderItemEntity orderItem = new OrderItemEntity();
            orderItem.OrderItemID = orderItemID;
            if (orderItemID > 0)
                if(_uow.OrderItemRepo.Delete(orderItem))
                {
                    _uow.SaveChanges();
                    return true;
                }
            return false;     
        }
        public IEnumerable<OrderItemDetailedDTO> GetOrderItemsDetailed(int orderHeaderID)
        {
            return _uow.OrderItemRepo.GetOrderItemsDetailedForOrder(orderHeaderID);            
        }
        public IEnumerable<OrderItemDTO> GetOrderItemsForOrder(int orderHeaderID)
        {
            return  _uow.OrderItemRepo.GetOrderItemsForOrder(orderHeaderID);
        }

        //Wrappers to other services
        public IEnumerable<int> GetDeliveryNotesForOrder(int orderID)
        {
            IDeliveryNoteService _deliveryNoteService = new DeliveryNoteService(_uow);
            return _deliveryNoteService.GetDeliveryNotesForOrder(orderID);
        }
        public IEnumerable<int> GetInvoicesForOrder(int orderHeaderID)
        {
            IInvoiceService _invoiceService = new InvoiceService(_uow);
            return _invoiceService.GetInvoicesForOrder(orderHeaderID);
        }
        public List<StockHierarchyItem> GetStockHierarchy()
        {
            IItemService _itemService = new ItemService(_uow);
            return _itemService.GetStockHierarchy();
        }
        public List<Customer> GetAvailableCustomers()
        {
            ICustomerService _customerService = new CustomerService(_uow);
            return _customerService.GetAll();
        }
        public List<AddressLocation> GetAvailableAddresses()
        {
            IAddressLocationService _addressService = new AddressLocationService(_uow);
            return _addressService.GetAll();
        }
        public List<CityArea>  GetAvailableCityAreas()
        {
            ICityAreaService _cityAreaService = new CityAreaService(_uow);
            return _cityAreaService.GetAll();
        }       
    }
}