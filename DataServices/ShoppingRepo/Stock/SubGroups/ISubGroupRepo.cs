using FMASolutionsCore.DataServices.DataRepository;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface ISubGroupRepo : IDataRepository<SubGroupEntity>
    {        
        SubGroupEntity GetByCode(string code);        
    }
}