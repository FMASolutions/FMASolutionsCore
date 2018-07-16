using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.DataRepository.Auth
{
    public class ApprovedAppsEntity : IBaseEntity
    {
        private Int32 _approvedAppID;

        public Int32 ID { get { return _approvedAppID; } set { _approvedAppID = value; } }
        public Int32 ApprovdAppID { get { return _approvedAppID; } protected set { _approvedAppID = value; } }
        public string AppName { get; protected set; }
        public string AppKey { get; protected set; }
        public string AppPassword { get; set; }
    }
}