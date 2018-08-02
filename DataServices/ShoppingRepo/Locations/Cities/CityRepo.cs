using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityRepo : BaseRepository, ICityRepo, IDisposable
    {
        public CityRepo(IDbTransaction transaction)
            :base(transaction){
            _dbConnection = Connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public CityEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [CityID], [CityCode], [CountryID], [CityName]
                FROM Cities
                WHERE CityID = @CityID
                ";

                Helper.logger.WriteToProcessLog("CityRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CityEntity>(query, new { CityID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<CityEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [CityID], [CityCode], [CountryID], [CityName]
                FROM Cities";

                Helper.logger.WriteToProcessLog("CityRepo.GetAll Started: " + query);

                return _dbConnection.Query<CityEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(CityEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO Cities([CityCode], [CountryID], CityName)
                VALUES (@CityCode, @CountryID, @CityName)";

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CityCode = entity.CityCode,
                    CountryID = entity.CountryID,
                    CityName = entity.CityName
                }, transaction: Transaction);

                Helper.logger.WriteToProcessLog("CityRepo.Create Started for code: " + entity.CityCode + " full query = " + query);

                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.Create: " + ex.Message, this);
                return false;
            }
        }

        public bool Update(CityEntity entity)
        {
            try
            {
                string query = @"
                UPDATE Cities 
                SET CityCode = @CityCode
                , CountryID = @CountryID
                , CityName = @CityName                
                WHERE CityID = @CityID";

                Helper.logger.WriteToProcessLog("CityRepo.Update Started for ID: " + entity.CityID + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CityCode = entity.CityCode,
                    CountryID = entity.CountryID,
                    CityName = entity.CityName,
                    CityID = entity.CityID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(CityEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICityRepo
        public Int32 GetNextAvailableID()
        {
            try
            {
                Helper.logger.WriteToProcessLog("CityRepo.GetNextAvailableID Started");
                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CityID),0)+1 FROM Cities", transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public CityEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CityID], [CityCode], [CountryID], [CityName]
                FROM Cities
                WHERE CityCode = @CityCode";

                Helper.logger.WriteToProcessLog("CityRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CityEntity>(query, new { CityCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CityRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}