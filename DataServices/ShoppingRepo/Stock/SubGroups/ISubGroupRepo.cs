using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ISubGroupRepo : IDataRepository<SubGroupEntity>
    {        
        SubGroupEntity GetByCode(string code);        
    }
}