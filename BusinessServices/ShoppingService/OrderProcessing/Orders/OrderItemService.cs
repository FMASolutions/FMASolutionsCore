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

        public IEnumerable<DTOOrderItemDetailed> GetOrderItemsDetailed(int orderHeaderID)
        {
            return _uow.OrderItemRepo.GetOrderItemsDetailedForOrder(orderHeaderID);            
        }
        public List<OrderItem> GetOrderItemsForOrder(int orderHeaderID)
        {
            var searchResults = _uow.OrderItemRepo.GetOrderItemsForOrder(orderHeaderID);
            if(searchResults != null)
            {
                List<OrderItem> returnItems = new List<OrderItem>();
                foreach(var item in searchResults)
                    returnItems.Add(ConvertItemEntityToModel(item));
            }
            return null;
        }



        private bool ValidateItemForCreate(OrderItem model)
        {
            //RUSHED NEEDS LOTS MORE WORK ON VALIDATION
            if(model.ItemID > 0 && model.OrderHeaderID > 0 && model.OrderItemQty > 0)
                return true;
            else
                return false;            
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
    
    }
}