using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IItemService : IDisposable
    {
        Item GetByID(int id);
        Item GetByCode(string code);
        bool CreateNew(Item model);
        List<Item> GetAll();
        List<SubGroup> GetAllAvailableSubGroups();
        bool UpdateDB(Item newModel);


    }
}