using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.DataRepository.Auth
{
    public class ApprovedAppsRepo : IDataRepository<ApprovedAppsEntity>
    {
        private IDbConnection _dbConnection;        
        public IDbConnection DBConnection {get {return _dbConnection;}}
        public ApprovedAppsRepo(IDbConnection connection)
        {
             _dbConnection = connection;
        }
        public bool Create(ApprovedAppsEntity entity)
        {
            throw new NotImplementedException();
        }

        public ApprovedAppsEntity GetByID(Int32 id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ApprovedAppsEntity> GetAll()
        {
            throw new NotImplementedException();
        }


        public ApprovedAppsEntity GetByAppKey(string appKey)
        {
            ApprovedAppsEntity entity;
            using (IDbConnection con = _dbConnection)
            {
                entity = con.QueryFirst<ApprovedAppsEntity>("SELECT AppName,AppKey,AppPassword FROM ApprovedApps WHERE AppKey = @AppKey", new { AppKey = appKey });
            }
            return entity;
        }

        public bool Update(ApprovedAppsEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ApprovedAppsEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}