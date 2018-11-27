using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class OrderService : IOrderService
    {
        public OrderService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _itemService = new ItemService(connectionString, dbType);
            _customerService = new CustomerService(connectionString, dbType);
            _addressService = new AddressLocationService(connectionString, dbType);
            _cityAreaService = new CityAreaService(connectionString, dbType);
            _customerAddressService = new CustomerAddressService(connectionString, dbType);
            _deliveryNoteService = new DeliveryNoteService(connectionString, dbType);
            _invoiceService = new InvoiceService(connectionString,dbType);
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
                _itemService.Dispose();
                _customerService.Dispose();
                _addressService.Dispose();
                _cityAreaService.Dispose();
                _customerAddressService.Dispose();
                _deliveryNoteService.Dispose();
                _invoiceService.Dispose();
                _uow.Dispose();
            }
        }

        private bool _disposing = false;
        private IUnitOfWork _uow;
        IItemService _itemService;
        ICustomerService _customerService;
        IAddressLocationService _addressService;
        ICityAreaService _cityAreaService;
        ICustomerAddressService _customerAddressService;
        IDeliveryNoteService _deliveryNoteService;
        IInvoiceService _invoiceService;

        //Direct Services
        public Order GetByID(int id)
        {
            Order returnOrder = null;
            OrderHeaderEntity headerEntity = _uow.OrderHeaderRepo.GetByID(id);

            if(headerEntity != null && headerEntity.OrderHeaderID > 0)
            {
                returnOrder = new Order();
                returnOrder.Header = ConvertHeaderEntityToModel(headerEntity);

                IEnumerable<OrderItemEntity> orderItems = _uow.OrderHeaderRepo.GetAllItemsForOrder(id);
                if(orderItems != null)
                {
                    foreach(var item in orderItems)
                    {
                        returnOrder.OrderItems.Add(ConvertItemEntityToModel(item));                        
                    }
                }
            }
            return returnOrder;
        }
        public int CreateOrder(OrderHeader model, AddressLocation newAddress = null)
        {   
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
        public int AddItemToOrder(OrderItem item)
        {
            try
            {
                bool success = false;
                if(ValidateItemForCreate(item))
                {
                    item.OrderItemStatusID = 1; //Estimate as new item
                    OrderItemEntity entity = ConvertItemModelToEntity(item);
                    success = _uow.OrderItemRepo.Create(item);
                    if(success)
                    {   
                        _uow.SaveChanges();                     
                        item.OrderItemID = _uow.OrderItemRepo.GetLatestOrderItemByOrder(item.OrderHeaderID);
                    }   
                    else
                    {                        
                        item.ModelState.AddError("CreateFailed","Unable to add Item To Order");
                        return -1;
                    }
                }
                return item.OrderItemID;
            }
            catch(Exception ex)
            {
                item.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return -1;
            }
        }
        public bool RemoveItemFromOrder(OrderItem item)
        {
            if (item.OrderItemID > 0)
                if(_uow.OrderItemRepo.Delete(ConvertItemModelToEntity(item)))
                {
                    _uow.SaveChanges();
                    return true;
                }            
                else
                    return false;
            else
            {
                item.ModelState.AddError("Unable To Remove","Unable to remove item from oreder, please specify OrderItemID");
                return false;
            }
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
        public bool UpdateHeader(Order newModel)
        {
            //Interesting?????
            return false;
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
            return _deliveryNoteService.GetDeliveryNotesForOrder(orderID);
        }
        public List<Invoice> GetInvoicesForOrder(int orderHeaderID)
        {
            return _invoiceService.GetInvoicesForOrder(orderHeaderID);
        }
        public List<StockHierarchyItem> GetStockHierarchy()
        {
            return _itemService.GetStockHierarchy();
        }
        public List<Customer> GetAvailableCustomers()
        {
            return _customerService.GetAll();
        }
        public List<AddressLocation> GetAvailableAddresses()
        {
            return _addressService.GetAll();
        }
        public List<CityArea>  GetAvailableCityAreas()
        {
            return _cityAreaService.GetAll();
        }
        public List<CustomerAddress> GetAvailableCustomerAddresses()
        {
            return _customerAddressService.GetAll();
        }        
        
        //PRivate Conversion
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
        private OrderItemEntity ConvertItemModelToEntity(OrderItem model)
        {
            return new OrderItemEntity(model.OrderItemID
                ,model.OrderHeaderID
                ,model.ItemID
                ,model.OrderItemStatusID
                ,model.OrderItemUnitPrice
                ,model.OrderItemUnitPriceAfterDiscount
                ,model.OrderItemQty
                ,model.OrderItemDescription
            );
        }
        private OrderItem ConvertItemEntityToModel(OrderItemEntity entity)
        {
            return new OrderItem(new CustomModelState()
                ,entity.ItemID
                ,entity.OrderItemStatusID
                ,entity.OrderHeaderID
                ,entity.OrderItemDescription
                ,entity.OrderItemID
                ,entity.OrderItemQty
                ,entity.OrderItemUnitPrice
                ,entity.OrderItemUnitPriceAfterDiscount
            );
        }
        
        //Validation
        private bool ValidateItemForCreate(OrderItem model)
        {
            //RUSHED NEEDS LOTS MORE WORK ON VALIDATION
            if(model.ItemID > 0 && model.OrderHeaderID > 0 && model.OrderItemQty > 0)
                return true;
            else
                return false;            
        }
    }
}