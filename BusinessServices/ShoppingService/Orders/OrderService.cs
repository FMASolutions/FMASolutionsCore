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
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _itemService.Dispose();
                _uow.Dispose();
            }
        }

        private bool _disposing = false;
        private IUnitOfWork _uow;
        IItemService _itemService;
        ICustomerService _customerService;

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
        public int CreateHeader(OrderHeader model)
        {
            try
            {
                bool success = false;
                if (ValidateHeaderForCreate(model))
                {
                    OrderHeaderEntity entity = ConvertHeaderModelToEntity(model);
                    success = _uow.OrderHeaderRepo.Create(entity);
                    if (success)
                    {   
                        _uow.SaveChanges();                     
                        OrderHeaderEntity createdModel = _uow.OrderHeaderRepo.GetLatestOrder();
                        model.OrderHeaderID = createdModel.OrderHeaderID;                        
                    }
                    else
                    {
                        model.ModelState.AddError("CreateFailed", "Unable to create new Order Header");
                        return -1;
                    }
                }
                return model.OrderHeaderID;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return -1;
            }            
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

        public DeliveryNote DeliverOrderItems(int orderHeaderID)
        {            
            DeliveryNoteEntity entity = _uow.DeliveryNoteRepo.DeliverOrder(orderHeaderID);
            DeliveryNote returnModel = ConvertDeliveryNoteToModel(entity);

            if(returnModel != null)
            {
                _uow.SaveChanges();
                return returnModel;
            }
            return null;
        }

        public List<StockHierarchyItem> GetStockHierarchy()
        {
            return _itemService.GetStockHierarchy();
        }
        public List<DeliveryNote> GetDeliveryNotesForOrder(int orderID)
        {
            List<DeliveryNote> returnList = new List<DeliveryNote>();

            IEnumerable<DeliveryNoteEntity> searchResult = _uow.DeliveryNoteRepo.GetByOrderHeaderID(orderID);

            foreach(var item in searchResult)
            {
                DeliveryNote currentItem = ConvertDeliveryNoteToModel(item);
                returnList.Add(currentItem);
            }

            return returnList;
        }
        public List<Customer> GetAvailableCustomers()
        {
            return _customerService.GetAll();
        }
        public Dictionary<int, string> GetOrderStatusDictionary()
        {
            Dictionary<int, string> returnDic = new Dictionary<int, string>();
            
            foreach(var item in _uow.OrderStatusRepo.GetAll())
                returnDic.Add(item.OrderStatusID, item.OrderStatusValue);

            return returnDic;
        }

        private OrderHeaderEntity ConvertHeaderModelToEntity(OrderHeader model)
        {
            //Int32 orderHeaderID, Int32 customerID, Int32 customerAddressID, Int32 orderStatusID, DateTime orderDate, DateTime deliveryDate
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

        private DeliveryNote ConvertDeliveryNoteToModel(DeliveryNoteEntity entity)
        {
            return new DeliveryNote( 
                new CustomModelState(),
                entity.Items,
                entity.DeliveryNoteID,
                entity.OrderHeaderID,
                entity.DeliveryDate
            );
        }

        private bool ValidateHeaderForCreate(OrderHeader model)
        {
            try
            {
                var custAddress = _uow.CustomerAddressRepo.GetByID(model.CustomerAddressID);
                var customer = _uow.CustomerRepo.GetByID(model.CustomerID);
                //Order Status eventually too.....
                
                if(custAddress != null && customer != null && model.OrderDate != null && model.DeliveryDate != null)
                {                    
                    return true;
                }
                return false;
            }
            catch(System.Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }

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
