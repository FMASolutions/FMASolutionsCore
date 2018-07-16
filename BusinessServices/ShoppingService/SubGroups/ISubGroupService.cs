using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface ISubGroupService
    {
        SubGroup GetByID(int id);
        SubGroup GetByCode(string code);
        bool CreateNew(SubGroup model);
        List<SubGroup> GetAll();
        List<ProductGroup> GetAvailableProductGroups();
        bool UpdateDB(SubGroup newModel);
    }
}