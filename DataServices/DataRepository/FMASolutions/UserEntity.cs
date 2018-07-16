using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.DataRepository.FMASolutions
{
    public class UserEntity : IBaseEntity
    {
        private Int32 _userID;

        public Int32 UserID { get { return _userID; } protected set { _userID = value; } }
        public Int32 ID { get { return _userID; } set { _userID = value; } }
        public string EmailAddress { get; set; }
        public int AuthTypeID { get; set; }
        public int UserRoleID { get; set; }
        public string KnownAs { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}