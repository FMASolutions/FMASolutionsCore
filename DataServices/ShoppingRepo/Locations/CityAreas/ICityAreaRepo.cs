using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICityAreaRepo : IDataRepository<CityAreaEntity>
    {           
        CityAreaEntity GetByCode(string code);
    }
}