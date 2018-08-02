using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class ProductGroupRepo : BaseRepository, IProductGroupRepo, IDisposable
    {
        public ProductGroupRepo(IDbTransaction transaction) 
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
        public ProductGroupEntity GetByID(Int32 id)
        {
            try
            {
                string query = @"
                SELECT [ProductGroupID], [ProductGroupCode],[ProductGroupName],[ProductGroupDescription] 
                FROM ProductGroups 
                WHERE ProductGroupID = @ProductGroupID
                ";

                Helper.logger.WriteToProcessLog("ProductGroupRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<ProductGroupEntity>(query, new { ProductGroupID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<ProductGroupEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [ProductGroupID], [ProductGroupCode],[ProductGroupName],[ProductGroupDescription] 
                FROM ProductGroups";

                Helper.logger.WriteToProcessLog("ProductGroupRepo.GetAll Started: " + query);

                return _dbConnection.Query<ProductGroupEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(ProductGroupEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO ProductGroups(ProductGroupCode, ProductGroupName, ProductGroupDescription)
                VALUES (@ProdGroupCode, @ProdGroupName, @ProdGroupDescription)";

                Helper.logger.WriteToProcessLog("ProductGroupRepo.Create Started for code: " + entity.ProductGroupCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    ProdGroupCode = entity.ProductGroupCode,
                    ProdGroupName = entity.ProductGroupName,
                    ProdGroupDescription = entity.ProductGroupDescription
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public bool Update(ProductGroupEntity entity)
        {
            try
            {
                string query = @"
                UPDATE ProductGroups 
                SET ProductGroupCode = @ProductGroupCode
                , ProductGroupName = @ProductGroupName
                , ProductGroupDescription = @ProductGroupDescription
                WHERE ProductGroupID = @ProductGroupID";

                Helper.logger.WriteToProcessLog("ProductGroupRepo.Update Started for ID: " + entity.ProductGroupID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    ProductGroupCode = entity.ProductGroupCode,
                    ProductGroupName = entity.ProductGroupName,
                    ProductGroupDescription = entity.ProductGroupDescription,
                    ProductGroupID = entity.ProductGroupID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(ProductGroupEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IProductGroupRepo
        public Int32 GetNextAvailableID()
        {
            try
            {
                Helper.logger.WriteToProcessLog("ProductGroupRepo.GetNextAvailableID Started");

                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(ProductGroupID),0)+1 FROM ProductGroups", transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public ProductGroupEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [ProductGroupID], [ProductGroupCode],[ProductGroupName],[ProductGroupDescription] 
                FROM ProductGroups 
                WHERE ProductGroupCode = @ProductGroupCode";

                Helper.logger.WriteToProcessLog("ProductGroupRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<ProductGroupEntity>(query, new { ProductGroupCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in ProductGroupRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}