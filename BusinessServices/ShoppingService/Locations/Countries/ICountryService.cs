using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICountryService : IDisposable
    {
        Country GetByID(int id);
        Country GetByCode(string code);
        bool CreateNew(Country model);
        List<Country> GetAll();
        bool UpdateDB(Country newModel);
    }
}