using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class CityAreaRepo : ICityAreaRepo, IDisposable
    {
        public CityAreaRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                return _dbConnection.QueryFirst<CityAreaEntity>(query, new { CityAreaID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<CityAreaEntity>(query);
            }
            catch (Exception)
            {
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

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    CityAreaCode = entity.CityAreaCode,
                    CityID = entity.CityID,
                    CityAreaName = entity.CityAreaName                    
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
                int i = _dbConnection.Execute(query, new
                {
                    CityAreaCode = entity.CityAreaCode,
                    CityID = entity.CityID,
                    CityAreaName = entity.CityAreaName,                    
                    CityAreaID = entity.CityAreaID
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

        public bool Delete(CityAreaEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICityAreaRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(CityAreaID),0)+1 FROM CityAreas");
        }
        public CityAreaEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [CityAreaID], [CityAreaCode], [CityID], [CityAreaName]
                FROM CityAreas
                WHERE CityAreaCode = @CityAreaCode";
                return _dbConnection.QueryFirst<CityAreaEntity>(query, new { CityAreaCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}