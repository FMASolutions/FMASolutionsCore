using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class ProductGroupEntity : IBaseEntity
    {
        public ProductGroupEntity()
        {
            _productGroupID = 0;
            _productGroupCode = "";
            _productGroupName = "";
            _productGroupDescription = "";            
        }
        public ProductGroupEntity(Int32 productGroupID, string productGroupCode, string productGroupName, string productGroupDescription)
        {
            _productGroupID = productGroupID;
            _productGroupCode = productGroupCode;
            _productGroupName = productGroupName;            
            _productGroupDescription = productGroupDescription;
        }

        protected Int32 _productGroupID;
        protected string _productGroupCode;
        protected string _productGroupName;
        protected string _productGroupDescription;
        public Int32 ID { get { return _productGroupID; } set { _productGroupID = value; } }
        public Int32 ProductGroupID { get { return _productGroupID; } set { _productGroupID = value; } }
        public string ProductGroupDescription { get => _productGroupDescription; set => _productGroupDescription = value; }
        public string ProductGroupName { get => _productGroupName; set => _productGroupName = value; }
        public string ProductGroupCode { get => _productGroupCode; set => _productGroupCode = value; }
    }
}