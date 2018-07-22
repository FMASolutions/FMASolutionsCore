using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityRepo : ICityRepo, IDisposable
    {
        public CityRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                return _dbConnection.QueryFirst<CityEntity>(query, new { CityID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<CityEntity>(query);
            }
            catch (Exception)
            {
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
                int i = _dbConnection.Execute(query, new
                {
                    CityCode = entity.CityCode,
                    CountryID = entity.CountryID,
                    CityName = entity.CityName,                    
                    CityID = entity.CityID
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

        public bool Delete(CityEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICityRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CityID),0)+1 FROM Cities");
        }
        public CityEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CityID], [CityCode], [CountryID], [CityName]
                FROM Cities
                WHERE CityCode = @CityCode";
                return _dbConnection.QueryFirst<CityEntity>(query, new { CityCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}