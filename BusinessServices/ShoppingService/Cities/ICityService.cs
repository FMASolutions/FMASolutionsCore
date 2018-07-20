using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICityService
    {
        City GetByID(int id);
        City GetByCode(string code);
        bool CreateNew(City model);
        List<City> GetAll();
        List<Country> GetAvailableCountries();
        bool UpdateDB(City newModel);
    }
}