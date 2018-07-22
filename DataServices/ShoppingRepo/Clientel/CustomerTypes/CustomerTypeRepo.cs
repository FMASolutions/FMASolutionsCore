using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerTypeRepo : ICustomerTypeRepo, IDisposable
    {
        public CustomerTypeRepo(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public CustomerTypeEntity GetByID(Int32 id)
        {
            try
            {
                string query = @"
                SELECT [CustomerTypeID], [CustomerTypeCode],[CustomerTypeName]
                FROM CustomerTypes
                WHERE CustomerTypeID = @CustomerTypeID";
                return _dbConnection.QueryFirst<CustomerTypeEntity>(query, new { CustomerTypeID = id });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<CustomerTypeEntity> GetAll()
        {
           try
            {
                string query = @"
                SELECT [CustomerTypeID], [CustomerTypeCode],[CustomerTypeName]
                FROM CustomerTypes";
                return _dbConnection.Query<CustomerTypeEntity>(query);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //These 3 should be moved to IUnit of Work
        public bool Create(CustomerTypeEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO CustomerTypes(CustomerTypeCode, CustomerTypeName)
                VALUES (@CustomerTypeCode, @CustomerTypeName)";

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerTypeCode = entity.CustomerTypeCode,
                    CustomerTypeName = entity.CustomerTypeName
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
        public bool Update(CustomerTypeEntity entity)
        {
            try
            {
                string query = @"
                UPDATE CustomerTypes 
                SET CustomerTypeCode = @CustomerTypeCode
                , CustomerTypeName = @CustomerTypeName                
                WHERE CustomerTypeID = @CustomerTypeID";
                int i = _dbConnection.Execute(query, new
                {
                    CustomerTypeCode = entity.CustomerTypeCode,
                    CustomerTypeName = entity.CustomerTypeName,                    
                    CustomerTypeID = entity.CustomerTypeID
                });
                if (i >= 1)
                    return true;
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Delete(CustomerTypeEntity entity)
        {
            throw new NotImplementedException();     
        }
        #endregion

        #region IProductGroupRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CustomerTypeID),0)+1 FROM CustomerTypes");
        }
        public CustomerTypeEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CustomerTypeID], [CustomerTypeCode],[CustomerTypeName]
                FROM CustomerTypes 
                WHERE CustomerTypeCode = @CustomerTypeCode";
                return _dbConnection.QueryFirst<CustomerTypeEntity>(query, new { CustomerTypeCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}