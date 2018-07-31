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

                Helper.logger.WriteToProcessLog("CustomerTypeRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerTypeEntity>(query, new { CustomerTypeID = id });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.GetByID: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("CustomerTypeRepo.GetAll Started: " + query);

                return _dbConnection.Query<CustomerTypeEntity>(query);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.GetAll: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("CustomerTypeRepo.Create Started for code: " + entity.CustomerTypeCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerTypeCode = entity.CustomerTypeCode,
                    CustomerTypeName = entity.CustomerTypeName
                });
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.Create: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("CustomerTypeRepo.Update Started for ID: " + entity.CustomerTypeID.ToString() + " full query = " + query);

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
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.Update: " + ex.Message, this);
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
            try
            {
                Helper.logger.WriteToProcessLog("CustomerTypeRepo.GetNextAvailableID Started");
                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CustomerTypeID),0)+1 FROM CustomerTypes");
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public CustomerTypeEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CustomerTypeID], [CustomerTypeCode],[CustomerTypeName]
                FROM CustomerTypes 
                WHERE CustomerTypeCode = @CustomerTypeCode";

                Helper.logger.WriteToProcessLog("CustomerTypeRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerTypeEntity>(query, new { CustomerTypeCode = code });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerTypeRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}