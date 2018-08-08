using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class AddressLocationRepo : BaseRepository, IAddressLocationRepo, IDisposable
    {
        public AddressLocationRepo(IDbTransaction transaction)
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
        public AddressLocationEntity GetByID(int id)
        {
            try
            {
                string query = @"
                SELECT [AddressLocationID],[AddressLocationCode],[AddressLine1],[AddressLine2],[CityAreaID],[PostCodeID]
                FROM AddressLocations
                WHERE AddressLocationID = @AddressLocationID
                ";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.GetByID Started for ID: " + id.ToString() + " full query = " + query);

                return _dbConnection.QueryFirst<AddressLocationEntity>(query, new { AddressLocationID = id }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.GetByID: " + ex.Message, this);
                return null;
            }
        }
        public IEnumerable<AddressLocationEntity> GetAll()
        {
            try
            {
                string query = @"
                SELECT [AddressLocationID],[AddressLocationCode],[AddressLine1],[AddressLine2],[CityAreaID],[PostCodeID]
                FROM AddressLocations";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.GetAll Started: " + query);

                return _dbConnection.Query<AddressLocationEntity>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.GetAll: " + ex.Message, this);
                return null;
            }
        }

        public bool Create(AddressLocationEntity entity)
        {
            try
            {
                string query = @"
                INSERT INTO AddressLocations([AddressLocationCode],[AddressLine1],[AddressLine2],[CityAreaID],[PostCodeID])
                VALUES (@AddressLocationCode, @AddressLine1, @AddressLine2, @CityAreaID, @PostCodeID)";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.Create Started for code: " + entity.AddressLocationCode + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    AddressLocationCode = entity.AddressLocationCode,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCodeID = entity.PostCodeID
                }, transaction: Transaction);
                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.Create: " + ex.Message, this);
                return false;
            }
        }

        public bool Update(AddressLocationEntity entity)
        {
            try
            {
                string query = @"
                UPDATE AddressLocations 
                SET AddressLocationCode = @AddressLocationCode
                , AddressLine1 = @AddressLine1
                , AddressLine2 = @AddressLine2
                , CityAreaID = @CityAreaID
                , PostCodeID = @PostCodeID
                WHERE AddressLocationID = @AddressLocationID";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.Update Started for ID: " + entity.AddressLocationID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {
                    AddressLocationCode = entity.AddressLocationCode,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCodeID = entity.PostCodeID,
                    AddressLocationID = entity.AddressLocationID
                }, transaction: Transaction);
                if (i >= 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.Update: " + ex.Message, this);
                return false;
            }
        }

        public bool Delete(AddressLocationEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IAddressLocationRepo
        public AddressLocationEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [AddressLocationID],[AddressLocationCode],[AddressLine1],[AddressLine2],[CityAreaID],[PostCodeID]
                FROM AddressLocations
                WHERE AddressLocationCode = @AddressLocationCode";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.GetByCode Started for Code: " + code + " full query = " + query);

                return _dbConnection.QueryFirst<AddressLocationEntity>(query, new { AddressLocationCode = code }, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.GetByCode: " + ex.Message, this);
                return null;
            }
        }
        #endregion
    }
}