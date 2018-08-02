using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IAddressLocationService : IDisposable
    {
        AddressLocation GetByID(int id);
        AddressLocation GetByCode(string code);
        bool CreateNew(AddressLocation model);
        bool CreateNew(AddressLocation newAddress, PostCode newPostCode);
        List<AddressLocation> GetAll();
        List<CityArea> GetAvailableCityAreas();
        List<PostCode> GetAvailablePostCodes();
        List<City> GetAvailableCities();
        bool UpdateDB(AddressLocation newModel);        
    }
}