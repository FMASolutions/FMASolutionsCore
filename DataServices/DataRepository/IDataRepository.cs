using System;
using System.Collections.Generic;
namespace FMASolutionsCore.DataServices.DataRepository
{
    public interface IDataRepository<T> where T : IBaseEntity
    {                           
        T GetByID(Int32 id);
        IEnumerable<T> GetAll();

        //The below 3 should eventuall be shipped out to IUnitOfWork Save Method
        bool Create(T entity);
        bool Delete(T entity);
        bool Update(T entity);
    }
}