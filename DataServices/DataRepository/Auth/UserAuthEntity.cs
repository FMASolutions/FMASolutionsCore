using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.DataRepository.Auth
{
    public class UserAuthEntity : IBaseEntity
    {
        private Int32 _userAuthID;
        private Int32 _userID;
        public Int32 UserAuthID { get { return _userAuthID; } protected set { _userAuthID = value; } }
        public Int32 UserID { get { return _userID; } protected set { _userID = value; } }
        public Int32 ID { get { return _userAuthID; } set { _userAuthID = value; } }
        public string UserKey { get; protected set; }
        public string Password { get; set; }
    }
}