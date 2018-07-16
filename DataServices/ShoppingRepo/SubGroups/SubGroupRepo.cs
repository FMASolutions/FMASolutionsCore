using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class SubGroupRepo : ISubGroupRepo, IDisposable
    {
        public SubGroupRepo(IDbConnection connection)
        {
            _dbConnection = connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public SubGroupEntity GetByID(int id)
        {
            try
            {

                string query = @"
                SELECT [SubGroupID], [SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription
                FROM SubGroups
                WHERE SubGroupID = @SubGroupID
                ";
                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupID = id });
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<SubGroupEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [SubGroupID], [SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription
                FROM SubGroups";
                return _dbConnection.Query<SubGroupEntity>(query);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Create(SubGroupEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO SubGroups([SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription)
                VALUES (@SubGroupCode, @ProductGroupID, @SubGroupName, @SubGroupDescription)";

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    SubGroupCode = entity.SubGroupCode,
                    ProductGroupID = entity.ProductGroupID,
                    SubGroupName = entity.SubGroupName,
                    SubGroupDescription = entity.SubGroupDescription
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

        public bool Update(SubGroupEntity entity)
        {
            try
            {
                string query = @"
                UPDATE SubGroups 
                SET SubGroupCode = @SubGroupCode
                , ProductGroupID = @ProductGroupID
                , SubGroupName = @SubGroupName
                , SubGroupDescription = @SubGroupDescription
                WHERE SubGroupID = @SubGroupID";
                int i = _dbConnection.Execute(query, new
                {
                    SubGroupCode = entity.SubGroupCode,
                    ProductGroupID = entity.ProductGroupID,
                    SubGroupName = entity.SubGroupName,
                    SubGroupDescription = entity.SubGroupDescription,
                    SubGroupID = entity.SubGroupID
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

        public bool Delete(SubGroupEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ISubGroupRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(SubGroupID),0)+1 FROM SubGroups");
        }
        public SubGroupEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [SubGroupID], [SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription
                FROM SubGroups 
                WHERE SubGroupCode = @SubGroupCode";
                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}