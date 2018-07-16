using System.Data;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.DataServices.DataRepository;
using System;
namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _connectionFactory = new SQLFactoryStandard();
            _dbConnection = _connectionFactory.CreateDBConnection(dbType,connectionString);
            _dbConnection.Open();
            _transaction = _dbConnection.BeginTransaction();
            _productGroupRepo = new ProductGroupRepo(new SQLFactoryStandard().CreateDBConnection(dbType,connectionString));
            _subGroupRepo = new SubGroupRepo(new SQLFactoryStandard().CreateDBConnection(dbType,connectionString));
            _itemRepo = new ItemRepo(new SQLFactoryStandard().CreateDBConnection(dbType,connectionString));
        }

        private IProductGroupRepo _productGroupRepo;
        private ISubGroupRepo _subGroupRepo;
        private IItemRepo _itemRepo;
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private SQLFactory _connectionFactory;


        public IProductGroupRepo ProductGroupRepository {get{return _productGroupRepo;}}
        public ISubGroupRepo SubGroupRepository {get { return _subGroupRepo;}}
        public IItemRepo ItemRepository {get { return _itemRepo;}}
        bool _disposed = false;

        public void SaveChanges(bool createFollowUpTransaction = true)
        {
            try
            {
                _transaction.Commit();
                if(createFollowUpTransaction)
                    _transaction = _dbConnection.BeginTransaction();
            }
            catch(Exception)
            {
                _transaction.Rollback();
                throw;                
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _dbConnection.BeginTransaction();
            }
        }
        public void Dispose()
        {
            if(!_disposed)
            {
                _dbConnection.Dispose();
                _transaction.Dispose();
                _disposed =true;
            }
        }
    }
}