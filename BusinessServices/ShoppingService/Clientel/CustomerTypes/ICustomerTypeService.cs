using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICustomerTypeService : IDisposable
    {
        CustomerType GetByID(int id);
        CustomerType GetByCode(string code);
        bool CreateNew(CustomerType model);
        List<CustomerType> GetAll();
        bool UpdateDB(CustomerType newModel);
    }
}