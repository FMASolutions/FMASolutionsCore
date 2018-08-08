using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CustomerAddressRepo : BaseRepository, ICustomerAddressRepo, IDisposable
    {
        public CustomerAddressRepo(IDbTransaction transaction)
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
        public CustomerAddressEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [CustomerAddressID], [CustomerAddressCode], [CustomerID], AddressLocationID, IsDefaultAddress, CustomerAddressDescription
                FROM CustomerAddresses
                WHERE CustomerAddressID = @CustomerAddressID
                ";

                Helper.logger.WriteToProcessLog("CustomerAddressRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerAddressEntity>(query, new { CustomerAddressID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerAddressRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<CustomerAddressEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [CustomerAddressID], [CustomerAddressCode], [CustomerID], AddressLocationID, IsDefaultAddress, CustomerAddressDescription
                FROM CustomerAddresses";

                Helper.logger.WriteToProcessLog("CustomerAddressRepo.GetAll Started: " + query);

                return _dbConnection.Query<CustomerAddressEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerAddressRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(CustomerAddressEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO CustomerAddresses([CustomerAddressCode], [CustomerID], AddressLocationID, IsDefaultAddress, CustomerAddressDescription)
                VALUES (@CustomerAddressCode, @CustomerID, @AddressLocationID, @IsDefaultAddress, @CustomerAddressDescription)";

                Helper.logger.WriteToProcessLog("CustomerAddressRepo.Create Started for code: " + entity.CustomerAddressCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CustomerAddressCode = entity.CustomerAddressCode,
                    CustomerID = entity.CustomerID,
                    AddressLocationID = entity.AddressLocationID,
                    IsDefaultAddress = entity.IsDefaultAddress,
                    CustomerAddressDescription = entity.CustomerAddressDescription
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerAddressRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public bool Update(CustomerAddressEntity entity)
        {
            try
            {
                string query = @"
                UPDATE CustomerAddresses 
                SET CustomerAddressCode = @CustomerAddressCode
                , CustomerID = @CustomerID
                , AddressLocationID = @AddressLocationID
                , IsDefaultAddress = @IsDefaultAddress
                , CustomerAddressDescription = @CustomerAddressDescription
                WHERE CustomerAddressID = @CustomerAddressID";

                Helper.logger.WriteToProcessLog("CustomerAddressRepo.Update Started for ID: " + entity.CustomerAddressID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CustomerAddressCode = entity.CustomerAddressCode,
                    CustomerID = entity.CustomerID,
                    AddressLocationID = entity.AddressLocationID,
                    IsDefaultAddress = entity.IsDefaultAddress,
                    CustomerAddressDescription = entity.CustomerAddressDescription,
                    CustomerAddressID = entity.CustomerAddressID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerAddressRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(CustomerAddressEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ISubGroupRepo
        public CustomerAddressEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CustomerAddressID], [CustomerAddressCode], [CustomerID], AddressLocationID, IsDefaultAddress, CustomerAddressDescription
                FROM CustomerAddresses
                WHERE CustomerAddressCode = @CustomerAddressCode";

                Helper.logger.WriteToProcessLog("CustomerAddressRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CustomerAddressEntity>(query, new { CustomerAddressCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CustomerAddressRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}