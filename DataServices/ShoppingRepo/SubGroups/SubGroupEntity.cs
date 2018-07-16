using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class SubGroupEntity : IBaseEntity
    {
        public SubGroupEntity()
        {            
            _subGroupID = 0;
            _productGroupID = 0;
            _subGroupCode = "";
            _subGroupName = "";
            _subGroupDescription = "";
        }
        public SubGroupEntity(Int32 subGroupID, Int32 productGroupID, string subGroupCode, string subGroupName, string subGroupDescription)
        {            
            _subGroupID = subGroupID;
            _productGroupID = productGroupID;
            _subGroupCode = subGroupCode;
            _subGroupName = subGroupName;
            _subGroupDescription = subGroupDescription;
        }

        protected Int32 _subGroupID;
        protected Int32 _productGroupID;
        protected string _subGroupCode;
        protected string _subGroupName;
        protected string _subGroupDescription;
        public Int32 ID { get { return _subGroupID; } set { _subGroupID = value; } }
        public Int32 SubGroupID { get { return _subGroupID; } set { _subGroupID = value; } }
        public Int32 ProductGroupID { get { return _productGroupID; } set { _productGroupID = value; } }
        public string SubGroupDescription { get => _subGroupDescription; set => _subGroupDescription = value; }
        public string SubGroupName { get => _subGroupName; set => _subGroupName = value; }
        public string SubGroupCode { get => _subGroupCode; set => _subGroupCode = value; }
    }
}