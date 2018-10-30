using FMASolutionsCore.DataServices.DataRepository;
using System;
using System.Collections.Generic;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IItemRepo : IDataRepository<ItemEntity>
    {        
        IEnumerable<StockHierarchyEntity> GetCompleteStockHierarchy();
        ItemEntity GetByCode(string code);
    }
}