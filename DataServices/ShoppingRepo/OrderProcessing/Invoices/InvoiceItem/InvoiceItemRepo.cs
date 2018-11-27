using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceItemRepo :  BaseRepository, IInvoiceItemRepo, IDisposable
    {
        public InvoiceItemRepo(IDbTransaction transaction)
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
        public bool Create(InvoiceItemEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO InvoiceItems(InvoiceHeaderID, OrderItemID, InvoiceItemStatusID, InvoiceItemQty)
                VALUES (@InvoiceHeaderID, @OrderItemID, @InvoiceItemStatusID, @InvoiceItemQty)";

                Helper.logger.WriteToProcessLog("InvoiceItemRepo.Create Started for OrderItemID: " + entity.OrderItemID.ToString() + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    InvoiceHeadID = entity.InvoiceHeaderID,
                    OrderItemID = entity.OrderItemID,
                    InvoiceItemStatusID = entity.InvoiceItemStatusID,
                    InvoiceItemQty = entity.InvoiceItemQty
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceItemRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public InvoiceItemEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT InvoiceItemID, InvoiceHeaderID, OrderItemID, InvoiceItemStatusID, InvoiceItemQty
                FROM InvoiceItems
                WHERE InvoiceItemID = @InvoiceItemID
                ";

                Helper.logger.WriteToProcessLog("InvoiceItemRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<InvoiceItemEntity>(query, new { OrderHeaderID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceItemRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<InvoiceItemEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT InvoiceItemID, InvoiceHeaderID, OrderItemID, InvoiceItemStatusID, InvoiceItemQty
                FROM InvoiceItems";

                Helper.logger.WriteToProcessLog("InvoiceItemRepo.GetAll Started: " + query);

                return _dbConnection.Query<InvoiceItemEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceItemRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }
        public bool Update(InvoiceItemEntity entity)
        {
            try
            {
                string query = @"
                UPDATE InvoiceItems
                SET InvoiceHeaderID = @InvoiceHeaderID
                , OrderItemID = @OrderItemID
                , InvoiceItemStatusID = @InvoiceItemStatusID
                , InvoiceItemQty = @InvoiceItemQty
                WHERE InvoiceItemID = @InvoiceItemID";

                Helper.logger.WriteToProcessLog("InvoiceItemRepo.Update Started for ID: " + entity.InvoiceItemID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    InvoiceHeaderID = entity.InvoiceHeaderID,
                    OrderItemID = entity.OrderItemID,
                    InvoiceItemStatusID = entity.InvoiceItemStatusID,
                    InvoiceItemQty = entity.InvoiceItemQty,
                    InvoiceItemID = entity.InvoiceItemID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceItemRepo.Update: " + ex.Message, this);
                return false;
            }
        }
        public bool Delete(InvoiceItemEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        public IEnumerable<InvoiceItemEntity> GetAllItemsForInvoice(int invoiceHeaderID)
        {
            try
            {
                string query = @"
                SELECT InvoiceItemID, InvoiceHeaderID, OrderItemID, InvoiceItemStatusID, InvoiceItemQty
                FROM InvoiceItems
                WHERE InvoiceHeaderID = @InvoiceHeaderID";

                Helper.logger.WriteToProcessLog("InvoiceItemRepo.GetAllItemsForInvoice Started: " + query);

                return _dbConnection.Query<InvoiceItemEntity>(query, new { InvoiceHeaderID = invoiceHeaderID }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceItemRepo.GetAllItemsForInvoice: " + ex.Message, this);
                return null;
            }
        }
    }
}