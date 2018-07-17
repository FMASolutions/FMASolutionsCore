using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICityRepo : IDataRepository<CityEntity>
    {
        Int32 GetNextAvailableID();                
        CityEntity GetByCode(string code);
    }
}