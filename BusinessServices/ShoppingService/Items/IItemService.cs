using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService.SubGroups;
namespace FMASolutionsCore.BusinessServices.ShoppingService.Items
{
    public interface IItemService
    {
        Item GetByID(int id);
        Item GetByCode(string code);
        bool CreateNew(Item model);
        List<Item> GetAll();
        List<SubGroup> GetAllAvailableSubGroups();
        bool UpdateDB(Item newModel);


    }
}