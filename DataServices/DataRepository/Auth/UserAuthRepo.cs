using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.DataRepository.Auth
{
    public class UserAuthRepo : IDataRepository<UserAuthEntity>
    {
        
        public UserAuthRepo(IDbConnection connection)
        {
            _dbConnection = connection;     
        }
        private IDbConnection _dbConnection;
        public IDbConnection DBConnection {get;}

        public bool Create(UserAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public UserAuthEntity GetByID(Int32 id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserAuthEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserAuthEntity GetByUserID(Int32 userID)
        {
            UserAuthEntity entity;
            using (IDbConnection con = DBConnection)
            {
                entity = con.QueryFirst<UserAuthEntity>("SELECT UserAuthID AS [id], UserID,UserKey,[Password] FROM UserAuth WHERE UserID = @UserID", new { UserID = userID });
            }
            return entity;
        }

        public UserAuthEntity GetByUsername(string username)
        {
            UserAuthEntity entity;
            using (IDbConnection con = DBConnection)
            {
                entity = con.QueryFirst<UserAuthEntity>("SELECT UA.UserAuthID AS [id], UA.UserID,UA.UserKey,UA.[Password] FROM UserAuth ua LEFT OUTER JOIN Users u on ua.UserID = u.UserID WHERE u.EmailAddress = @EmailAddress", new { EmailAddress = username });
            }
            return entity;
        }

        public bool Update(UserAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserAuthEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}