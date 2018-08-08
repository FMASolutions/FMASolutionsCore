using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICityRepo : IDataRepository<CityEntity>
    {       
        CityEntity GetByCode(string code);
    }
}