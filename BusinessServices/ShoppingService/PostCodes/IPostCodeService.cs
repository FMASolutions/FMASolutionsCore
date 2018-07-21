using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IPostCodeService
    {
        PostCode GetByID(int id);
        PostCode GetByCode(string code);
        bool CreateNew(PostCode model);
        List<PostCode> GetAll();
        List<City> GetAvailableCities();
        bool UpdateDB(PostCode newModel);
    }
}