using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IAddressLocationService
    {
        AddressLocation GetByID(int id);
        AddressLocation GetByCode(string code);
        bool CreateNew(AddressLocation model);
        List<AddressLocation> GetAll();
        List<CityArea> GetAvailableCityAreas();
        List<PostCode> GetAvailablePostCodes();
        bool UpdateDB(AddressLocation newModel);
    }
}