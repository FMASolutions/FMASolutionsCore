using System;
using Dapper;
using System.Data;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.DataRepository.FMASolutions
{
    public abstract class UserRepo : IDataRepository<UserEntity>
    {        
        public UserRepo(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public IDbConnection DBConnection {get {return _dbConnection;}}
        private IDbConnection _dbConnection;



        public bool Create(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetByID(Int32 id)
        {
            UserEntity entity;
            using (IDbConnection con = DBConnection)
            {
                entity = con.QueryFirst<UserEntity>("SELECT UserID AS [id]"
                    + ",EmailAddress,AuthTypeID,UserRoleID,KnownAs,Firstname"
                    + ",Surname,MobileNumber,AddressLine1,AddressLine2"
                    + ",AddressLine3,City,PostCode"
                    + " FROM Users WHERE UserID = @UserID", new { UserID = id });
            }
            return entity;
        }
        public IEnumerable<UserEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserEntity GetByEmail(string emailAddress)
        {
            UserEntity entity;
            using (IDbConnection con = DBConnection)
            {
                entity = con.QueryFirst<UserEntity>("SELECT UserID AS [id]"
                    + ",EmailAddress,AuthTypeID,UserRoleID,KnownAs,Firstname"
                    + ",Surname,MobileNumber,AddressLine1,AddressLine2"
                    + ",AddressLine3,City,PostCode"
                    + " FROM Users WHERE EmailAddress = @EmailAddress", new { EMailAddress = emailAddress });
            }

            return entity;
        }

        public bool Update(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}