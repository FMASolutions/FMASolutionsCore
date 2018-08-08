using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerAddressRepo : IDataRepository<CustomerAddressEntity>
    {        
        CustomerAddressEntity GetByCode(string code);
    }
}