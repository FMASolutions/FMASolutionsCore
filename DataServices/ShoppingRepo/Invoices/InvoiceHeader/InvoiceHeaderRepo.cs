using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceHeaderRepo : BaseRepository, IInvoiceHeaderRepo, IDisposable
    {
        public InvoiceHeaderRepo(IDbTransaction transaction)
            : base(transaction)
        {
            _dbConnection = transaction.Connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public bool Create(InvoiceHeaderEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO InvoiceHeaders(OrderHeaderID, InvoiceStatusID, InvoiceDate)
                VALUES (@OrderHeaderID, @InvoiceStatusID, @InvoiceDate)";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.Create Started for OrderHeaderID: " + entity.OrderHeaderID.ToString() + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    OrderHeaderID = entity.OrderHeaderID,
                    InvoiceStatusID = entity.InvoiceStatusID,
                    InvoiceDate = entity.InvoiceDate
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public InvoiceHeaderEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT InvoiceHeaderID, OrderHeaderID, InvoiceStatusID, InvoiceDate
                FROM InvoiceHeaders
                WHERE InvoiceHeaderID = @InvoiceHeaderID
                ";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<InvoiceHeaderEntity>(query, new { InvoiceHeaderID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<InvoiceHeaderEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT InvoiceHeaderID, OrderHeaderID, InvoiceStatusID, InvoiceDate
                FROM InvoiceHeaders";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.GetAll Started: " + query);

                return _dbConnection.Query<InvoiceHeaderEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }
        public bool Update(InvoiceHeaderEntity entity)
        {
            try
            {
                string query = @"
                UPDATE InvoiceHeaders 
                SET OrderHeaderID = @OrderHeaderID
                , InvoiceStatusID = @InvoiceStatusID
                , InvoiceDate = @InvoiceDate
                WHERE InvoiceHeaderID = @InvoiceHeaderID";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.Update Started for ID: " + entity.InvoiceHeaderID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    InvoiceHeaderID = entity.InvoiceHeaderID,
                    OrderHeaderID = entity.OrderHeaderID,
                    InvoiceStatusID = entity.InvoiceStatusID,
                    InvoiceDate = entity.InvoiceDate
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.Update: " + ex.Message, this);
                return false;
            }
        }
        public bool Delete(InvoiceHeaderEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        public int GetLatestInvoiceHeaderID()
        {
            try
            {
                string query = @"
                SELECT TOP 1 InvoiceHeaderID
                FROM InvoiceHeaders
                ORDER BY InvoiceHeaderID desc";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.GetLatestInvoiceHeaderID Started: " + query);

                return _dbConnection.QueryFirst<int>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.GetLatestInvoiceHeaderID: " + ex.Message, this);
                return 0;
            }
        }

        public int GenerateInvoiceForOrder(int orderHeaderID)
        {
            try
            {
                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.DeliverOrder Started for ID: " + orderHeaderID.ToString());
                if(orderHeaderID > 0)
                {                
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@OrderHeaderID", orderHeaderID);
                    return _dbConnection.QueryFirst<int>("GenerateInvoiceForOrder",queryParameters,transaction: Transaction, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<int> GetInvoicesForOrder(int orderHeaderID)
        {
            try
            {
                string query = @"
                SELECT InvoiceHeaderID
                FROM InvoiceHeaders
                WHERE OrderHeaderID = @OrderHeaderID";

                Helper.logger.WriteToProcessLog("InvoiceHeaderRepo.GetInvoicesForOrder Started for OrderID: " + orderHeaderID.ToString() + " full query := " + query);

                return _dbConnection.Query<int>(query,new { OrderHeaderID = orderHeaderID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceHeaderRepo.GetInvoicesForOrder: " + ex.Message, this);
                return null;
            }
        }
    }
}
