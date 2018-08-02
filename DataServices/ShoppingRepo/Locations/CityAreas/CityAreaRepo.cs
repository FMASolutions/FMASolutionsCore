using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityAreaRepo : BaseRepository, ICityAreaRepo, IDisposable
    {
        public CityAreaRepo(IDbTransaction transaction)
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
        public CityAreaEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [CityAreaID], [CityAreaCode], [CityID], [CityAreaName]
                FROM CityAreas
                WHERE CityAreaID = @CityAreaID
                ";

                Helper.logger.WriteToProcessLog("CityAreaRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CityAreaEntity>(query, new { CityAreaID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<CityAreaEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [CityAreaID], [CityAreaCode], [CityID], [CityAreaName]
                FROM CityAreas";

                Helper.logger.WriteToProcessLog("CityAreaRepo.GetAll Started: " + query);

                return _dbConnection.Query<CityAreaEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(CityAreaEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO CityAreas([CityAreaCode], [CityID], [CityAreaName])
                VALUES (@CityAreaCode, @CityID, @CityAreaName)";

                Helper.logger.WriteToProcessLog("CityAreaRepo.Create Started for code: " + entity.CityAreaCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CityAreaCode = entity.CityAreaCode,
                    CityID = entity.CityID,
                    CityAreaName = entity.CityAreaName
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.Create: " + ex.Message, this);
                return false;
            }
        }

        public bool Update(CityAreaEntity entity)
        {
            try
            {
                string query = @"
                UPDATE CityAreas 
                SET CityAreaCode = @CityAreaCode
                , CityID = @CityID
                , CityAreaName = @CityAreaName                
                WHERE CityAreaID = @CityAreaID";

                Helper.logger.WriteToProcessLog("CityAreaRepo.Update Started for ID: " + entity.CityAreaID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CityAreaCode = entity.CityAreaCode,
                    CityID = entity.CityID,
                    CityAreaName = entity.CityAreaName,
                    CityAreaID = entity.CityAreaID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(CityAreaEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICityAreaRepo
        public Int32 GetNextAvailableID()
        {
            try
            {
                Helper.logger.WriteToProcessLog("CityAreaRepo.GetNextAvailableID Started");
                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CityAreaID),0)+1 FROM CityAreas", transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public CityAreaEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CityAreaID], [CityAreaCode], [CityID], [CityAreaName]
                FROM CityAreas
                WHERE CityAreaCode = @CityAreaCode";

                Helper.logger.WriteToProcessLog("CityAreaRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CityAreaEntity>(query, new { CityAreaCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityAreaRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}