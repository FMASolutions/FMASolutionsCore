using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICountryRepo : IDataRepository<CountryEntity>
    {
        CountryEntity GetByCode(string code);
    }
}