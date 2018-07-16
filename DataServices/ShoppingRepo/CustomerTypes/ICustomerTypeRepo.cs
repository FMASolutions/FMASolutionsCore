using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerTypeRepo : IDataRepository<CustomerTypeEntity>
    {
        Int32 GetNextAvailableID();
        CustomerTypeEntity GetByCode(string code);                
    }

}