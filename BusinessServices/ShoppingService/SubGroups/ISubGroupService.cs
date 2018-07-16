using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
namespace FMASolutionsCore.BusinessServices.ShoppingService.SubGroups
{
    public interface ISubGroupService
    {
        SubGroup GetByID(int id);
        SubGroup GetByCode(string code);
        bool CreateNew(SubGroup model);
        List<SubGroup> GetAll();
        List<ProductGroups.ProductGroup> GetAvailableProductGroups();
        bool UpdateDB(SubGroup newEntity);
    }
}