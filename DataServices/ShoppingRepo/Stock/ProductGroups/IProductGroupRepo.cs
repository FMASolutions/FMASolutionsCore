using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IProductGroupRepo : IDataRepository<ProductGroupEntity>
    {
        Int32 GetNextAvailableID();
        ProductGroupEntity GetByCode(string code);
    }
}