using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IAddressLocationService : IDisposable
    {
        AddressLocation GetByID(int id);        
        bool CreateNew(AddressLocation model);        
        List<AddressLocation> GetAll();
        List<CityArea> GetAvailableCityAreas();        
        bool UpdateDB(AddressLocation newModel);               
    }
}