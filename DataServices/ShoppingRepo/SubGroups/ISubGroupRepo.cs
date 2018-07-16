using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ISubGroupRepo : IDataRepository<SubGroupEntity>
    {
        Int32 GetNextAvailableID();                
        SubGroupEntity GetByCode(string code);        
    }
}