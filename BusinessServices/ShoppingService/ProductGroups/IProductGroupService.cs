using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
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