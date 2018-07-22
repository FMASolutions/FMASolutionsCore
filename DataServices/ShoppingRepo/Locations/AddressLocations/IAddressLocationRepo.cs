using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IAddressLocationRepo : IDataRepository<AddressLocationEntity>
    {
        Int32 GetNextAvailableID();                
        AddressLocationEntity GetByCode(string code);
    }
}