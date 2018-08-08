using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ICustomerAddressService : IDisposable
    {
        CustomerAddress GetByID(int id);
        CustomerAddress GetByCode(string code);
        bool CreateNew(CustomerAddress model);
        List<CustomerAddress> GetAll();
        List<Customer> GetAvailableCustomers();
        List<AddressLocation> GetAvailableAddressLocations();
        bool UpdateDB(CustomerAddress newModel);
    }
}