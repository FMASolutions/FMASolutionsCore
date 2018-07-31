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

                Helper.logger.WriteToProcessLog("PostCodeRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<PostCodeEntity>(query, new { PostCodeID = id });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.GetByID: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("PostCodeRepo.GetAll Started: " + query);

                return _dbConnection.Query<PostCodeEntity>(query);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.GetAll: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("PostCodeRepo.Create Started for VALUE: " + entity.PostCodeValue + " full query = " + query);

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
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.Create: " + ex.Message, this);
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

                Helper.logger.WriteToProcessLog("PostCodeRepo.Update Started for ID: " + entity.PostCodeID.ToString() + " full query = " + query);

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
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.Update: " + ex.Message, this);
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
            try
            {
                Helper.logger.WriteToProcessLog("PostCodeRepo.GetNextAvailableID Started");
                return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(PostCodeID),0)+1 FROM PostCodes");
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.GetNextAvailableID: " + ex.Message, this);
                return -1;
            }
        }
        public PostCodeEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [PostCodeID],[PostCodeCode],[CityID],[PostCodeValue]
                FROM PostCodes
                WHERE PostCodeCode = @PostCodeCode";

                Helper.logger.WriteToProcessLog("PostCodeRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<PostCodeEntity>(query, new { PostCodeCode = code });
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in PostCodeRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}