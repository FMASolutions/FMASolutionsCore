using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class SubGroupRepo : BaseRepository, ISubGroupRepo, IDisposable
    {
        public SubGroupRepo(IDbTransaction transaction)
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

                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupID = id }, transaction: Transaction);
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

                return _dbConnection.Query<SubGroupEntity>(query, transaction: Transaction);
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
                }, transaction: Transaction);
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
                }, transaction: Transaction);
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
        public SubGroupEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [SubGroupID], [SubGroupCode], [ProductGroupID], SubGroupName, SubGroupDescription
                FROM SubGroups 
                WHERE SubGroupCode = @SubGroupCode";

                Helper.logger.WriteToProcessLog("SubGroupRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<SubGroupEntity>(query, new { SubGroupCode = code }, transaction: Transaction);
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