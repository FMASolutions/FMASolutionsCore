using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

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

        public int DeliverOrderItems(int orderHeaderID)
        {            
            int deliveryNoteID = _uow.DeliveryNoteRepo.DeliverOrder(orderHeaderID);

            if(deliveryNoteID > 0)
                _uow.SaveChanges();
            
            return deliveryNoteID;
        }
        public IEnumerable<int> GetDeliveryNotesForOrder(int orderID)
        {
            return _uow.DeliveryNoteRepo.GetByOrderHeaderID(orderID);
        }

        public IEnumerable<DeliveryNoteItemDTO> GetDeliveryNoteByDeliveryNoteID(int deliveryNoteID)
        {
            return _uow.DeliveryNoteRepo.GetDeliveryNoteItems(deliveryNoteID);
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