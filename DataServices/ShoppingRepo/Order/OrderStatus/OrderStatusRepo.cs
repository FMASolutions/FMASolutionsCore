using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderStatusRepo : BaseRepository, IOrderStatusRepo, IDisposable
    {
        public OrderStatusRepo(IDbTransaction transaction)
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
        public OrderStatusEntity GetByID(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<OrderStatusEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT OrderStatusID, OrderStatusValue
                FROM OrderStatus";

                Helper.logger.WriteToProcessLog("OrderStatusEntity.GetAll Started: " + query);

                return _dbConnection.Query<OrderStatusEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderStatusEntity.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(OrderStatusEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool Update(OrderStatusEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(OrderStatusEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}