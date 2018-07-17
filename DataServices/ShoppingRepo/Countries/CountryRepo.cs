using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CountryRepo : ICountryRepo, IDisposable
    {
        public CountryRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                return _dbConnection.QueryFirst<CountryEntity>(query, new { CountryID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<CountryEntity>(query);
            }
            catch (Exception)
            {
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

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CountryCode = entity.CountryCode,
                    CountryName = entity.CountryName
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
        public bool Update(CountryEntity entity)
        {
            try
            {
                string query = @"
                UPDATE Countries 
                SET CountryCode = @CountryCode
                , CountryName = @CountryName
                WHERE CountryID = @CountryID";
                int i = _dbConnection.Execute(query, new
                {
                    CountryCode = entity.CountryCode,
                    CountryName = entity.CountryName,                    
                    CountryID = entity.CountryID
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

        public bool Delete(CountryEntity entity)
        {
            throw new NotImplementedException();     
        }
        #endregion

        #region ICountryRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CountryID),0)+1 FROM Countries");
        }
        public CountryEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CountryID], [CountryCode],[CountryName]
                FROM Countries 
                WHERE CountryCode = @CountryCode";
                return _dbConnection.QueryFirst<CountryEntity>(query, new { CountryCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }    
}