using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerAddressRepo : IDataRepository<CustomerAddressEntity>
    {  
        int GetMostRecent();
        int GetCustomerAddressIDByCustomerAndAddress(int customerID, int addressID);
    }
}