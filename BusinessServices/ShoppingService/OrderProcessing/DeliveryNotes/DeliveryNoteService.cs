using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class DeliveryNoteService : IDeliveryNoteService
    {
        public DeliveryNoteService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            
        }
        internal DeliveryNoteService(IUnitOfWork uow)
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
        public DeliveryNote GetDeliveryNoteByID(int deliveryNoteID)
        {
            DeliveryNote returnNote;
            DeliveryNoteEntity entity = _uow.DeliveryNoteRepo.GetByID(deliveryNoteID);
            if(entity != null)
            {
                returnNote = ConvertDeliveryNoteToModel(entity);
                return returnNote;
            }
            return null;
        }
        public Order GetOrder(int orderHeaderID)
        {
            IOrderService orderService = new OrderService(_uow);
            return orderService.GetByID(orderHeaderID);
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
    }
}