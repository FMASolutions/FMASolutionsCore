using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IItemRepo : IDataRepository<ItemEntity>
    {        
        ItemEntity GetByCode(string code);
    }
}