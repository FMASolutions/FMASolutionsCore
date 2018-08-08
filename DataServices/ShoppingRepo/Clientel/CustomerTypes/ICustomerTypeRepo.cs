using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ICustomerTypeRepo : IDataRepository<CustomerTypeEntity>
    {
        CustomerTypeEntity GetByCode(string code);
    }

}