using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class OrderHeaderRepo : BaseRepository, IOrderHeaderRepo, IDisposable
    {
        public OrderHeaderRepo(IDbTransaction transaction)
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
        public OrderHeaderEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT OrderHeaderID,CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate
                FROM OrderHeaders
                WHERE OrderHeaderID = @OrderHeaderID
                ";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<OrderHeaderEntity>(query, new { OrderHeaderID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<OrderHeaderEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT OrderHeaderID,CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate
                FROM OrderHeaders";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.GetAll Started: " + query);

                return _dbConnection.Query<OrderHeaderEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(OrderHeaderEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO OrderHeaders(CustomerID,CustomerAddressID,OrderStatusID,OrderDate,DeliveryDate)
                VALUES (@CustomerID, @CustomerAddressID, @OrderStatusID, @OrderDate,@DeliveryDate)";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.Create Started for Customer ID: " + entity.CustomerID + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerID = entity.CustomerID,
                    CustomerAddressID = entity.CustomerAddressID,
                    OrderStatusID = entity.OrderStatusID,
                    OrderDate = entity.OrderDate,
                    DeliveryDate = entity.DeliveryDate
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public bool Update(OrderHeaderEntity entity)
        {
            try
            {
                string query = @"
                UPDATE OrderHeaders 
                SET CustomerID = @CustomerID
                , CustomerAddressID = @CustomerAddressID
                , OrderStatusID = @OrderStatusID
                , OrderDate = @OrderDate
                , DeliveryDate = @DeliveryDate
                WHERE OrderHeaderID = @OrderHeaderID";

                Helper.logger.WriteToProcessLog("OrderHeaderRepo.Update Started for ID: " + entity.OrderHeaderID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CustomerID = entity.CustomerID,
                    CustomerAddressID = entity.CustomerAddressID,
                    OrderStatusID = entity.OrderStatusID,
                    OrderDate = entity.OrderDate,
                    DeliveryDate = entity.DeliveryDate,
                    OrderHeaderID = entity.OrderHeaderID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in OrderHeaderRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(OrderHeaderEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}