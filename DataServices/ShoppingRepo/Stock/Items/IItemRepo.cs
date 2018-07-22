using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IItemRepo : IDataRepository<ItemEntity>
    {
        Int32 GetNextAvailableID();                
        ItemEntity GetByCode(string code);
    }
}