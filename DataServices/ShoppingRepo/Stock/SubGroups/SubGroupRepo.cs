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

                Helper.logger.WriteToProcessLog("SubGroupRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupID = id });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.GetByID: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("SubGroupRepo.GetAll Started: " + query);

                return _dbConnection.Query<SubGroupEntity>(query);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.GetAll: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("SubGroupRepo.Create Started for code: " + entity.SubGroupCode + " full query = " + query);

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
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.Create: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("SubGroupRepo.Update Started for ID: " + entity.SubGroupID.ToString() + " full query = " + query);

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
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.Update: " + ex.Message, this);
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
            try
            {
                Helper.logger.WriteToProcessLog("SubGroupRepo.GetNextAvailableID Started");

                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(SubGroupID),0)+1 FROM SubGroups");
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public SubGroupEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [SubGroupID], [SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription
                FROM SubGroups 
                WHERE SubGroupCode = @SubGroupCode";

                Helper.logger.WriteToProcessLog("SubGroupRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupCode = code });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in SubGroupRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}