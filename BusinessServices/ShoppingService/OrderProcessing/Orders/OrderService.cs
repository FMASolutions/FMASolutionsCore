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


        //Direct Services
        public Order GetByID(int id)
        {
            Order returnOrder = null;
            OrderHeaderEntity headerEntity = _uow.OrderHeaderRepo.GetByID(id);

            if(headerEntity != null && headerEntity.OrderHeaderID > 0)
            {                
                returnOrder = new Order();                
                IOrderItemService _itemService = new OrderItemService(_uow);
                List<OrderItem> searchItems = _itemService.GetOrderItemsForOrder(id);
                returnOrder.Header = ConvertHeaderEntityToModel(headerEntity);
                if(searchItems != null && searchItems.Count > 0)
                    returnOrder.OrderItems = searchItems;
            }
            return returnOrder;
        }
        public int CreateOrder(OrderHeader model, AddressLocation newAddress = null)
        {   
            IAddressLocationService _addressService = new AddressLocationService(_uow);
            ICustomerAddressService _customerAddressService = new CustomerAddressService(_uow);

            OrderHeaderEntity entityToCreate = ConvertHeaderModelToEntity(model);

            if(newAddress != null)
            {
                bool addressCreated = _addressService.CreateNew(newAddress);
                int newAddressID = _uow.AddressLocationRepo.GetMostRecent();

                CustomerAddress customerAddress = new CustomerAddress(
                    new CustomModelState(), 
                    0,
                    model.CustomerID,
                    newAddressID,
                    true,
                    "AUTO"
                );
                int newCustomerAddressID = 0;
                if(_customerAddressService.CreateNew(customerAddress))
                    newCustomerAddressID = _uow.CustomerAddressRepo.GetMostRecent();
                if(newCustomerAddressID != 0)
                    entityToCreate.CustomerAddressID = newCustomerAddressID;                    
            }
            
            else //Use Existing Address (might also be a CustomerAddress so check that first...)
            {
                int customerAddressIDSearch = _uow.CustomerAddressRepo.GetCustomerAddressIDByCustomerAndAddress(
                    model.CustomerID
                    , model.CustomerAddressID //CustomerAddressID is passed as that is what the WebUI passes the AddressID as...
                );
                if(customerAddressIDSearch > 0)
                {
                    entityToCreate.CustomerAddressID = customerAddressIDSearch;
                }
                else
                {
                    CustomerAddress customerAddress = new CustomerAddress(
                    new CustomModelState(), 
                    0,
                    model.CustomerID,
                    model.CustomerAddressID,
                    true,
                    "AUTO"
                    );

                    int newCustAddID = 0;
                    if(_customerAddressService.CreateNew(customerAddress))
                    {
                        newCustAddID = _uow.CustomerAddressRepo.GetCustomerAddressIDByCustomerAndAddress(model.CustomerID,model.CustomerAddressID);
                    }
                    entityToCreate.CustomerAddressID = newCustAddID;
                }
            }

            bool createSuccess = _uow.OrderHeaderRepo.Create(entityToCreate);
            if(createSuccess)
            {
                _uow.SaveChanges();
                return _uow.OrderHeaderRepo.GetLatestOrder().OrderHeaderID;
            }
            else
                return 0;
        }        
        public List<Order> GetAllOrders()
        {
            var headers = _uow.OrderHeaderRepo.GetAll();

            if(headers != null)            
            {
                List<Order> returnOrders = new List<Order>();
                foreach(var header in headers)
                {
                    Order newOrder = GetByID(header.OrderHeaderID);
                    returnOrders.Add(newOrder);
                }
                return returnOrders;
            }
            else
            {
                return null;
            }            
        }    
        public DTOOrderHeaderDetailed GetOrderHeaderDetailed(int orderHeaderID)
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
        

        //Wrappers to other services
        public List<DeliveryNote> GetDeliveryNotesForOrder(int orderID)
        {
            IDeliveryNoteService _deliveryNoteService = new DeliveryNoteService(_uow);
            return _deliveryNoteService.GetDeliveryNotesForOrder(orderID);
        }
        public List<Invoice> GetInvoicesForOrder(int orderHeaderID)
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
        public List<CustomerAddress> GetAvailableCustomerAddresses()
        {
            ICustomerAddressService _customerAddressService = new CustomerAddressService(_uow);
            return _customerAddressService.GetAll();
        }        
        

        //Private Conversion
        private OrderHeaderEntity ConvertHeaderModelToEntity(OrderHeader model)
        {            
            return new OrderHeaderEntity(model.OrderHeaderID
                ,model.CustomerID
                ,model.CustomerAddressID
                ,model.OrderStatusID
                ,model.OrderDate
                ,model.DeliveryDate
            );            
        }
        private OrderHeader ConvertHeaderEntityToModel(OrderHeaderEntity entity)
        {
            return new OrderHeader(new CustomModelState()
                , entity.CustomerAddressID
                , entity.CustomerID
                ,entity.DeliveryDate
                ,entity.OrderDate
                ,entity.OrderHeaderID
                ,entity.OrderStatusID
            );
        }
    }
}