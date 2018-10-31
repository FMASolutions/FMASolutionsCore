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
        IAddressLocationRepo AddressLocationRepo {get;}
        ICustomerAddressRepo CustomerAddressRepo {get;}
        ICustomerRepo CustomerRepo {get;}
        IOrderHeaderRepo OrderHeaderRepo {get;}
        IOrderItemRepo OrderItemRepo {get;}    
        IOrderStatusRepo OrderStatusRepo {get;}    

        void SaveChanges();
    }
}