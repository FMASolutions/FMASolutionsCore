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
                SELECT [AddressLocationID],[AddressLine1],[AddressLine2],[CityAreaID],[PostCode]
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
                SELECT [AddressLocationID],[AddressLine1],[AddressLine2],[CityAreaID],[PostCode]
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
                INSERT INTO AddressLocations([AddressLine1],[AddressLine2],[CityAreaID],[PostCode])
                VALUES (@AddressLine1, @AddressLine2, @CityAreaID, @PostCode)";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.Create Started for Address: " + entity.AddressLine1 + " " + entity.AddressLine2 + " full query = " + query);

                int rowsAffected = _dbConnection.Execute(query, new
                {                    
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCode = entity.PostCode
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
                SET AddressLine1 = @AddressLine1
                , AddressLine2 = @AddressLine2
                , CityAreaID = @CityAreaID
                , PostCode = @PostCode
                WHERE AddressLocationID = @AddressLocationID";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.Update Started for ID: " + entity.AddressLocationID.ToString() + " full query = " + query);

                int i = _dbConnection.Execute(query, new
                {                    
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCode = entity.PostCode,
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

        #region BaseRepository
        public int GetMostRecent()
        {
            try
            {
                string query = @"
                SELECT TOP 1 [AddressLocationID]
                FROM AddressLocations
                ORDER BY AddressLocationID Desc";

                Helper.logger.WriteToProcessLog("AddressLocationRepo.GetMostRecent Started, full query = " + query);               

                return _dbConnection.QueryFirst<int>(query, transaction: Transaction);
            }
            catch (Exception ex)
            {
                Helper.logger.WriteToErrorLog("Error in AddressLocationRepo.GetMostRecent: " + ex.Message, this);
                return 0;
            }
        }
        #endregion
    }
}