using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IPostCodeRepo : IDataRepository<PostCodeEntity>
    {
        Int32 GetNextAvailableID();                
        PostCodeEntity GetByCode(string code);
    }
}