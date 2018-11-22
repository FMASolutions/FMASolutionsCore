using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IAddressLocationRepo : IDataRepository<AddressLocationEntity>
    {        
        int GetMostRecent();
        
    }
}