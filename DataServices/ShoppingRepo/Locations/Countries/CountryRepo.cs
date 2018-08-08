using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CountryRepo : BaseRepository, ICountryRepo, IDisposable
    {
        public CountryRepo(IDbTransaction transaction)
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
        public CountryEntity GetByID(Int32 id)
        {
            try
            {
                string query = @"
                SELECT [CountryID], [CountryCode],[CountryName]
                FROM Countries
                WHERE CountryID = @CountryID";

                Helper.logger.WriteToProcessLog("CountryRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<CountryEntity>(query, new { CountryID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CountryRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }

        public IEnumerable<CountryEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [CountryID], [CountryCode],[CountryName]
                FROM Countries";

                Helper.logger.WriteToProcessLog("CountryRepo.GetAll Started: " + query);

                return _dbConnection.Query<CountryEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CountryRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        //These 3 should be moved to IUnit of Work
        public bool Create(CountryEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO Countries(CountryCode, CountryName)
                VALUES (@CountryCode, @CountryName)";

                Helper.logger.WriteToProcessLog("CountryRepo.Create Started for code: " + entity.CountryCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CountryCode = entity.CountryCode,
                    CountryName = entity.CountryName
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CountryRepo.Create: " + ex.Message, this);
                return false;
            }
        }
        public bool Update(CountryEntity entity)
        {
            try
            {
                string query = @"
                UPDATE Countries 
                SET CountryCode = @CountryCode
                , CountryName = @CountryName
                WHERE CountryID = @CountryID";

                Helper.logger.WriteToProcessLog("CountryRepo.Update Started for ID: " + entity.CountryID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    CountryCode = entity.CountryCode,
                    CountryName = entity.CountryName,
                    CountryID = entity.CountryID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CountryRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(CountryEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICountryRepo
        public CountryEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CountryID], [CountryCode],[CountryName]
                FROM Countries 
                WHERE CountryCode = @CountryCode";

                Helper.logger.WriteToProcessLog("Country.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<CountryEntity>(query, new { CountryCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in CountryRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}