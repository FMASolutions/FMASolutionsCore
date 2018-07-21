using System;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class AddressLocationRepo : IAddressLocationRepo, IDisposable
    {
        public AddressLocationRepo(IDbConnection connection)
        {
            _dbConnection = connection;
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
                return _dbConnection.QueryFirst<AddressLocationEntity>(query, new { AddressLocationID = id });
            }
            catch (Exception)
            {
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
                return _dbConnection.Query<AddressLocationEntity>(query);
            }
            catch (Exception)
            {
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

                int rowsAffected = _dbConnection.Execute(query, new
                {
                    AddressLocationCode = entity.AddressLocationCode,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCodeID = entity.PostCodeID
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
                int i = _dbConnection.Execute(query, new
                {
                    AddressLocationCode = entity.AddressLocationCode,
                    AddressLine1 = entity.AddressLine1,
                    AddressLine2 = entity.AddressLine2,
                    CityAreaID = entity.CityAreaID,
                    PostCodeID = entity.PostCodeID,
                    AddressLocationID = entity.AddressLocationID
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

        public bool Delete(AddressLocationEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IAddressLocationRepo
        public Int32 GetNextAvailableID()
        {
            return _dbConnection.QueryFirst<Int32>("SELECT ISNULL(MAX(AddressLocationID),0)+1 FROM AddressLocations");
        }
        public AddressLocationEntity GetByCode(string code)
        {
            try
            {
                string query = @"
                SELECT [AddressLocationID],[AddressLocationCode],[AddressLine1],[AddressLine2],[CityAreaID],[PostCodeID]
                FROM AddressLocations
                WHERE AddressLocationCode = @AddressLocationCode";
                return _dbConnection.QueryFirst<AddressLocationEntity>(query, new { AddressLocationCode = code });
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}