using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICustomerService
    {
        Customer GetByID(int id);
        Customer GetByCode(string code);
        bool CreateNew(Customer model);
        List<Customer> GetAll();
        List<CustomerType> GetAvailableCustomerTypes();
        bool UpdateDB(Customer newModel);
    }
}