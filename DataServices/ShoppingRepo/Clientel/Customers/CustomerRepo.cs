using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerRepo : ICustomerRepo, IDisposable
    {
        public CustomerRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                return _dbConnection.QueryFirst<CustomerEntity>(query, new { CustomerID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<CustomerEntity>(query);
            }
            catch (Exception)
            {
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

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerTypeID = entity.CustomerTypeID,
                    CustomerCode = entity.CustomerCode,
                    CustomerName = entity.CustomerName,
                    CustomerContactNumber = entity.CustomerContactNumber,
                    CustomerEmailAddress = entity.CustomerEmailAddress
                });
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
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
                int i = _dbConnection.Execute(query, new
                {
                    CustomerTypeID = entity.CustomerTypeID,
                    CustomerCode = entity.CustomerCode,
                    CustomerName = entity.CustomerName,
                    CustomerContactNumber = entity.CustomerContactNumber,
                    CustomerEmailAddress = entity.CustomerEmailAddress,
                    CustomerID = entity.CustomerID                    
                });
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception)
            {
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
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CustomerID),0)+1 FROM Customers");
        }
        public CustomerEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CustomerID], [CustomerTypeID], [CustomerCode], [CustomerName], [CustomerContactNumber], [CustomerEmailAddress]
                FROM Customers 
                WHERE CustomerCode = @CustomerCode";
                return _dbConnection.QueryFirst<CustomerEntity>(query, new { CustomerCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}