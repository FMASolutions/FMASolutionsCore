using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
namespace FMASolutionsCore.BusinessServices.ShoppingService.ProductGroups
{
    public interface IProductGroupService
    {
        ProductGroup GetByID(int id);
        ProductGroup GetByCode(string code);
        bool CreateNew(ProductGroup entityToCreate);
        List<ProductGroup> GetAll();
        bool UpdateDB(ProductGroup newEntity);
    }
}