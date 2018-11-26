using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceStatusRepo : BaseRepository, IInvoiceStatusRepo, IDisposable
    {
        public InvoiceStatusRepo(IDbTransaction transaction)
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
        public InvoiceStatusEntity GetByID(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<InvoiceStatusEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT InvoiceStatusID, InvoiceStatusValue
                FROM InvoiceStatus";

                Helper.logger.WriteToProcessLog("InvoiceStatusEntity.GetAll Started: " + query);

                return _dbConnection.Query<InvoiceStatusEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in InvoiceStatusEntity.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(InvoiceStatusEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool Update(InvoiceStatusEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(InvoiceStatusEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}