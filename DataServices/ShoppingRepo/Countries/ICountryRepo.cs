using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICountryRepo : IDataRepository<CountryEntity>
    {
        Int32 GetNextAvailableID();
        CountryEntity GetByCode(string code);
    }
}