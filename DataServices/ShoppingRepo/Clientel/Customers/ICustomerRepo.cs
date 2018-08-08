using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerRepo : IDataRepository<CustomerEntity>
    {
        CustomerEntity GetByCode(string code);
    }
}