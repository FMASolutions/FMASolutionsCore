using System.Data;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IUnitOfWork : IDisposable
    {
        IProductGroupRepo ProductGroupRepository {get; }
        ISubGroupRepo SubGroupRepository {get;}
        IItemRepo ItemRepository {get;}
        ICustomerTypeRepo CustomerTypeRepo {get;}

        void SaveChanges(bool createFollowUpTransaction = true);
    }
}