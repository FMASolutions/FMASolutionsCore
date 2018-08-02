using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerRepo : BaseRepository, ICustomerRepo, IDisposable
    {
        public CustomerRepo(IDbTransaction transaction)
            :base(transaction)
        {
            _dbConnection = Connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public CustomerEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [CustomerID], [CustomerTypeID], [CustomerCode], [CustomerName], [CustomerContactNumber], [CustomerEmailAddress]
                FROM Customers
                WHERE CustomerID = @CustomerID
                ";

                Helper.logger.WriteToProcessLog("CustomerRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerEntity>(query, new { CustomerID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<CustomerEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [CustomerID], [CustomerTypeID], [CustomerCode], [CustomerName], [CustomerContactNumber], [CustomerEmailAddress]
                FROM Customers";

                Helper.logger.WriteToProcessLog("CustomerRepo.GetAll Started: " + query);

                return _dbConnection.Query<CustomerEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(CustomerEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO Customers([CustomerTypeID], [CustomerCode], [CustomerName], [CustomerContactNumber], [CustomerEmailAddress])
                VALUES (@CustomerTypeID, @CustomerCode, @CustomerName, @CustomerContactNumber, @CustomerEmailAddress)";

                Helper.logger.WriteToProcessLog("CustomerRepo.Create Started for code: " + entity.CustomerCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerTypeID = entity.CustomerTypeID,
                    CustomerCode = entity.CustomerCode,
                    CustomerName = entity.CustomerName,
                    CustomerContactNumber = entity.CustomerContactNumber,
                    CustomerEmailAddress = entity.CustomerEmailAddress
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.Create: " + ex.Message, this);
                return false;
            }
        }

        public bool Update(CustomerEntity entity)
        {
            try
            {
                string query = @"
                UPDATE Customers 
                SET CustomerTypeID = @CustomerTypeID
                , CustomerCode = @CustomerCode
                , CustomerName = @CustomerName
                , CustomerContactNumber = @CustomerContactNumber
                , CustomerEmailAddress = @CustomerEmailAddress
                WHERE CustomerID = @CustomerID";

                Helper.logger.WriteToProcessLog("CustomerRepo.Update Started for ID: " + entity.CustomerID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CustomerTypeID = entity.CustomerTypeID,
                    CustomerCode = entity.CustomerCode,
                    CustomerName = entity.CustomerName,
                    CustomerContactNumber = entity.CustomerContactNumber,
                    CustomerEmailAddress = entity.CustomerEmailAddress,
                    CustomerID = entity.CustomerID                    
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICustomerRepo
        public Int32 GetNextAvailableID()
        {
            try
            {
                Helper.logger.WriteToProcessLog("CustomerRepo.GetNextAvailableID Started");
                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CustomerID),0)+1 FROM Customers", transaction: Transaction);
            }
            catch(Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public CustomerEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CustomerID], [CustomerTypeID], [CustomerCode], [CustomerName], [CustomerContactNumber], [CustomerEmailAddress]
                FROM Customers 
                WHERE CustomerCode = @CustomerCode";

                Helper.logger.WriteToProcessLog("CustomerRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerEntity>(query, new { CustomerCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}