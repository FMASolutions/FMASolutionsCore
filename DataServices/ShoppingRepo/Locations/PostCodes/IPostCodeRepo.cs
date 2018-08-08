using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IPostCodeRepo : IDataRepository<PostCodeEntity>
    {        
        PostCodeEntity GetByCode(string code);
    }
}