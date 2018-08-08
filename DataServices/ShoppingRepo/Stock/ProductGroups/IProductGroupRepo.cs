using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IProductGroupRepo : IDataRepository<ProductGroupEntity>
    {
        ProductGroupEntity GetByCode(string code);
    }
}