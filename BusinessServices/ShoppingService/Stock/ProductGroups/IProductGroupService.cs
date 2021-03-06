using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IProductGroupService : IDisposable
    {
        ProductGroup GetByID(int id);
        ProductGroup GetByCode(string code);
        bool CreateNew(ProductGroup model);
        List<ProductGroup> GetAll();
        bool UpdateDB(ProductGroup newModel);
    }
}