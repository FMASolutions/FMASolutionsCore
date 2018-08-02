using System.Data;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public abstract class BaseRepository
    {
        public BaseRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        protected IDbConnection Connection {get { return Transaction.Connection; }}
        protected IDbTransaction Transaction {get; private set;}
    }
}