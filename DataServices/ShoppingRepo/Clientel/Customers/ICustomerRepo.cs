using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerRepo : IDataRepository<CustomerEntity>
    {
        Int32 GetNextAvailableID();
        CustomerEntity GetByCode(string code);
    }
}