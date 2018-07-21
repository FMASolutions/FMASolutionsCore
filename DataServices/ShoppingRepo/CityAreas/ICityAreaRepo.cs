using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICityAreaRepo : IDataRepository<CityAreaEntity>
    {
        Int32 GetNextAvailableID();                
        CityAreaEntity GetByCode(string code);
    }
}