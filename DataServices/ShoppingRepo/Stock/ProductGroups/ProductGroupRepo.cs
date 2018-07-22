using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class ProductGroupRepo : IProductGroupRepo, IDisposable
    {
        public ProductGroupRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                WHERE ProductGroupID = @ProductGroupID";
                return _dbConnection.QueryFirst<ProductGroupEntity>(query, new { ProductGroupID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<ProductGroupEntity>(query);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //These 3 should be moved to IUnit of Work
        public bool Create(ProductGroupEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO ProductGroups(ProductGroupCode, ProductGroupName, ProductGroupDescription)
                VALUES (@ProdGroupCode, @ProdGroupName, @ProdGroupDescription)";

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    ProdGroupCode = entity.ProductGroupCode,
                    ProdGroupName = entity.ProductGroupName,
                    ProdGroupDescription = entity.ProductGroupDescription
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
                int i = _dbConnection.Execute(query, new
                {
                    ProductGroupCode = entity.ProductGroupCode,
                    ProductGroupName = entity.ProductGroupName,
                    ProductGroupDescription = entity.ProductGroupDescription,
                    ProductGroupID = entity.ProductGroupID
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

        public bool Delete(ProductGroupEntity entity)
        {
            throw new NotImplementedException();     
        }
        #endregion

        #region IProductGroupRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(ProductGroupID),0)+1 FROM ProductGroups");
        }
        public ProductGroupEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [ProductGroupID], [ProductGroupCode],[ProductGroupName],[ProductGroupDescription] 
                FROM ProductGroups 
                WHERE ProductGroupCode = @ProductGroupCode";
                return _dbConnection.QueryFirst<ProductGroupEntity>(query, new { ProductGroupCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}