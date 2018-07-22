using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICityAreaService
    {
        CityArea GetByID(int id);
        CityArea GetByCode(string code);
        bool CreateNew(CityArea model);
        List<CityArea> GetAll();
        List<City> GetAvailableCities();
        bool UpdateDB(CityArea newModel);
    }
}