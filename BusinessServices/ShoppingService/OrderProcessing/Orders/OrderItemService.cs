using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class OrderItemService : IOrderItemService
    {
        public OrderItemService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
        }
        internal OrderItemService(IUnitOfWork uow)
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


        public int AddItemToOrder(OrderItemCreationDTO item)
        {
            bool success = false;
            if(ValidateItemForCreate(item))
            {
                item.OrderItemStatusID = 1; //Estimate as new item
                OrderItemEntity entity = new OrderItemEntity(0, item.OrderHeaderID, item.ItemID, item.OrderItemStatusID
                    ,item.OrderItemUnitPrice, item.OrderItemUnitPriceAfterDiscount, item.OrderItemQty, item.OrderItemDescription);
                success = _uow.OrderItemRepo.Create(entity);
                if(success)
                {   
                    _uow.SaveChanges();                     
                    return _uow.OrderItemRepo.GetLatestOrderItemByOrder(item.OrderHeaderID);
                }
            }
            return -1;            
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


        private bool ValidateItemForCreate(OrderItemCreationDTO model)
        {
            //RUSHED NEEDS LOTS MORE WORK ON VALIDATION
            if(model.ItemID > 0 && model.OrderHeaderID > 0 && model.OrderItemQty > 0)
                return true;
            else
                return false;            
        }
    }
}