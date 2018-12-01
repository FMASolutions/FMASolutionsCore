
using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

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
            try
            {
                string query = @"
                SELECT DN.DeliveryNoteID, DN.OrderHeaderID, DN.DeliveryDate, DNI.OrderItemID, DNI.DeliveryNoteItemID
                FROM DeliveryNotes DN
                INNER JOIN DeliveryNoteItems DNI ON DN.DeliveryNoteID = DNI.DeliveryNoteID
                WHERE DN.DeliveryNoteID = @DeliveryNoteID
                ";

                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                var pocoList = _dbConnection.Query<DeliveryNotePOCO>(query, new { DeliveryNoteID = id }, transaction: Transaction);
                
                DeliveryNoteEntity returnEntity = new DeliveryNoteEntity();
                bool firstFlag = true;
                foreach(var item in pocoList)
                {
                    if(firstFlag)
                    {       
                        firstFlag = false;
                        returnEntity.DeliveryNoteID = item.DeliveryNoteID;
                        returnEntity.OrderHeaderID = item.OrderHeaderID;
                        returnEntity.DeliveryDate = item.DeliveryDate;
                    }
                    DeliveryNoteEntity.DeliveryNoteItem current = new DeliveryNoteEntity.DeliveryNoteItem();
                    current.DeliveryNoteItemID = item.DeliveryNoteItemID;
                    current.DeliveryNoteID = item.DeliveryNoteID;
                    current.OrderItemID = item.OrderItemID;
                    
                    returnEntity.Items.Add(current);
                }
                
                if(returnEntity.DeliveryNoteID > 0 && returnEntity.Items.Count > 0)
                  return returnEntity;
                else
                  return null;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<DeliveryNoteEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT DN.DeliveryNoteID, DN.OrderHeaderID, DN.DeliveryDate, DNI.OrderItemID, DNI.DeliveryNoteItemID
                FROM DeliveryNotes DN
                INNER JOIN DeliveryNoteItems DNI ON DN.DeliveryNoteID = DNI.DeliveryNoteID
                ORDER BY DN.DeliveryNoteID DESC
                ";
                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.GetAll Started, full query = " + query);

                var pocoList = _dbConnection.Query<DeliveryNotePOCO>(query, transaction: Transaction);
                
                
                List<DeliveryNoteEntity> returnEntity = new List<DeliveryNoteEntity>();
                DeliveryNoteEntity currentEntity = null;
                                
                bool first = true;
                int currentDeliveryNoteID = 0;
                
                foreach(var item in pocoList)
                {
                    if(currentDeliveryNoteID != item.DeliveryNoteID)
                    {       
                        currentDeliveryNoteID = item.DeliveryNoteID;
                        if(first)                        
                            first = false;
                        else
                            returnEntity.Add(currentEntity);
                        
                        currentEntity = new DeliveryNoteEntity();
                        currentEntity.DeliveryNoteID = item.DeliveryNoteID;
                        currentEntity.OrderHeaderID = item.OrderHeaderID;
                        currentEntity.DeliveryDate = item.DeliveryDate;
                    }
                    DeliveryNoteEntity.DeliveryNoteItem currentItem = new DeliveryNoteEntity.DeliveryNoteItem();
                    currentItem.DeliveryNoteItemID = item.DeliveryNoteItemID;
                    currentItem.DeliveryNoteID = item.DeliveryNoteID;
                    currentItem.OrderItemID = item.OrderItemID;             
                    currentEntity.Items.Add(currentItem);
                }
                returnEntity.Add(currentEntity); 
                
                if(returnEntity.Count > 0)
                  return returnEntity;
                else
                  return null;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public IEnumerable<DeliveryNoteEntity> GetByOrderHeaderID(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT DN.DeliveryNoteID, DN.OrderHeaderID, DN.DeliveryDate, DNI.OrderItemID, DNI.DeliveryNoteItemID
                FROM DeliveryNotes DN
                INNER JOIN DeliveryNoteItems DNI ON DN.DeliveryNoteID = DNI.DeliveryNoteID
                WHERE DN.OrderHeaderID = @OrderHeaderID
                ORDER BY DN.DeliveryNoteID DESC
                ";
                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.GetAll Started, full query = " + query);

                var pocoList = _dbConnection.Query<DeliveryNotePOCO>(query, new { OrderHeaderID = orderHeaderID }, transaction: Transaction);
                
                
                List<DeliveryNoteEntity> returnEntity = new List<DeliveryNoteEntity>();
                DeliveryNoteEntity currentEntity = null;
                                
                bool first = true;
                int currentDeliveryNoteID = 0;
                
                foreach(var item in pocoList)
                {
                    if(currentDeliveryNoteID != item.DeliveryNoteID)
                    {       
                        currentDeliveryNoteID = item.DeliveryNoteID;
                        if(first)                        
                            first = false;
                        else
                            returnEntity.Add(currentEntity);
                        
                        currentEntity = new DeliveryNoteEntity();
                        currentEntity.DeliveryNoteID = item.DeliveryNoteID;
                        currentEntity.OrderHeaderID = item.OrderHeaderID;
                        currentEntity.DeliveryDate = item.DeliveryDate;
                    }
                    DeliveryNoteEntity.DeliveryNoteItem currentItem = new DeliveryNoteEntity.DeliveryNoteItem();
                    currentItem.DeliveryNoteItemID = item.DeliveryNoteItemID;
                    currentItem.DeliveryNoteID = item.DeliveryNoteID;
                    currentItem.OrderItemID = item.OrderItemID;             
                    currentEntity.Items.Add(currentItem);
                }
                returnEntity.Add(currentEntity); 
                
                if(returnEntity.Count > 0)
                  return returnEntity;
                else
                  return null;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.GetAll: " + ex.Message, this);
                return null;
            }
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
        
        public DeliveryNoteEntity DeliverOrder(int orderHeaderID)
        {
            try
            {
                Helper.logger.WriteToProcessLog("DeliveryNoteRepo.DeliverOrder Started for ID: " + orderHeaderID.ToString());
                if(orderHeaderID > 0)
                {                
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@OrderHeaderID", orderHeaderID);
                    int deliveryNoteID = _dbConnection.QueryFirst<int>("DeliverExistingItems",queryParameters,transaction: Transaction, commandType: CommandType.StoredProcedure);
                    return GetByID(deliveryNoteID);
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in DeliveryNoteRepo.DeliverOutstandingItems: " + ex.Message, this);
                return null;
            }
        }
        
    }
}