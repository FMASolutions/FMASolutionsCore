using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class PostCodeRepo : IPostCodeRepo, IDisposable
    {
        public PostCodeRepo(IDbConnection connection)
        {
            _dbConnection = connection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private IDbConnection _dbConnection;

        #region IDataRepository
        public PostCodeEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [PostCodeID],[PostCodeCode],[CityID],[PostCodeValue]
                FROM PostCodes
                WHERE PostCodeID = @PostCodeID
                ";
                return _dbConnection.QueryFirst<PostCodeEntity>(query, new { PostCodeID = id });
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<PostCodeEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [PostCodeID],[PostCodeCode],[CityID],[PostCodeValue]
                FROM PostCodes";
                return _dbConnection.Query<PostCodeEntity>(query);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Create(PostCodeEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO PostCodes([PostCodeCode],[CityID],[PostCodeValue])
                VALUES (@PostCodeCode, @CityID, @PostCodeValue)";

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    PostCodeCode = entity.PostCodeCode,
                    CityID = entity.CityID,
                    PostCodeValue = entity.PostCodeValue                    
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

        public bool Update(PostCodeEntity entity)
        {
            try
            {
                string query = @"
                UPDATE PostCodes 
                SET PostCodeCode = @PostCodeCode
                , CityID = @CityID
                , PostCodeValue = @PostCodeValue
                WHERE PostCodeID = @PostCodeID";
                int i = _dbConnection.Execute(query, new
                {
                    PostCodeCode = entity.PostCodeCode,
                    CityID = entity.CityID,
                    PostCodeValue = entity.PostCodeValue,  
                    PostCodeID = entity.PostCodeID
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

        public bool Delete(PostCodeEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IPostCodeRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(PostCodeID),0)+1 FROM PostCodes");
        }
        public PostCodeEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [PostCodeID],[PostCodeCode],[CityID],[PostCodeValue]
                FROM PostCodes
                WHERE PostCodeCode = @PostCodeCode";
                return _dbConnection.QueryFirst<PostCodeEntity>(query, new { PostCodeCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}