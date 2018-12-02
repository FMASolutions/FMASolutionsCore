
using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class DeliveryNoteRepo : BaseRepository, IDeliveryNoteRepo, IDisposable
    {
        public DeliveryNoteRepo(IDbTransaction transaction)
           : base(transaction)
        {
            _dbConnection = Connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public DeliveryNoteEntity GetByID(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<DeliveryNoteEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Create(DeliveryNoteEntity entity)
        {
            throw new NotImplementedException(); //CREATE IS HANDLED VIA STORED PROC ON DELIVERY OF ORDER
        }
        public bool Update(DeliveryNoteEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(DeliveryNoteEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion     
        
        public IEnumerable<int> GetByOrderHeaderID(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT DN.DeliveryNoteID
                FROM DeliveryNotes DN
                WHERE DN.OrderHeaderID = @OrderHeaderID
                ORDER BY DN.DeliveryNoteID DESC
                ";
                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.GetAll Started, full query = " + query);
                return _dbConnection.Query<int>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public int DeliverOrder(int orderHeaderID)
        {
            try
            {
                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.DeliverOrder Started for ID: " + orderHeaderID.ToString());
                if(orderHeaderID > 0)
                {                
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@OrderHeaderID", orderHeaderID);
                    int deliveryNoteID = _dbConnection.QueryFirst<int>("DeliverExistingItems",queryParameters,transaction: Transaction, commandType: CommandType.StoredProcedure);
                    return deliveryNoteID;
                }
                else
                    return 0;
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.DeliverOutstandingItems: " + ex.Message, this);
                return 0;
            }
        }
        public IEnumerable<DeliveryNoteItemDTO> GetDeliveryNoteItems(int deliveryNoteID)
        {
            try
            {
                string query = @"
                SELECT di.DeliveryNoteItemID, di.DeliveryNoteID, oi.OrderHeaderID, dn.DeliveryDate, di.OrderItemID, oi.OrderItemDescription, oi.OrderItemQty
                FROM DeliveryNoteItems di
                INNER JOIN DeliveryNotes dn ON dn.DeliveryNoteID = di.DeliveryNoteID
                INNER JOIN OrderItems oi ON di.OrderItemID = oi.OrderItemID
                WHERE di.DeliveryNoteID = @DeliveryNoteID
                ";

                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.GetDeliveryNoteItems Started: " + query);

                return _dbConnection.Query<DeliveryNoteItemDTO>(query, new { DeliveryNoteID = deliveryNoteID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.GetDeliveryNoteItems: " + ex.Message, this);
                return null;
            }
        }
    }
}
