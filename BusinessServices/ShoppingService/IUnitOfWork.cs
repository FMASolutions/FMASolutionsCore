using System.Data;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IUnitOfWork : IDisposable
    {
        IProductGroupRepo ProductGroupRepo {get; }
        ISubGroupRepo SubGroupRepo {get;}
        IItemRepo ItemRepo {get;}
        ICustomerTypeRepo CustomerTypeRepo {get;}
        ICountryRepo CountryRepo {get;}
        ICityRepo CityRepo {get;}
        ICityAreaRepo CityAreaRepo {get;}
        IPostCodeRepo PostCodeRepo {get;}
        IAddressLocationRepo AddressLocationRepo {get;}
        ICustomerAddressRepo CustomerAddressRepo {get;}
        ICustomerRepo CustomerRepo {get;}

        void SaveChanges();
    }
}