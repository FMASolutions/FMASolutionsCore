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
            _dbConnection = _connectionFactory.CreateDBConnection(dbType, connectionString);
            _dbConnection.Open();
            _transaction = _dbConnection.BeginTransaction();
            _productGroupRepo = new ProductGroupRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _subGroupRepo = new SubGroupRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _itemRepo = new ItemRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _customerTypeRepo = new CustomerTypeRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _countryRepo = new CountryRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _cityRepo = new CityRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
            _cityAreaRepo = new CityAreaRepo(new SQLFactoryStandard().CreateDBConnection(dbType, connectionString));
        }

        private IProductGroupRepo _productGroupRepo;
        private ISubGroupRepo _subGroupRepo;
        private IItemRepo _itemRepo;
        private ICustomerTypeRepo _customerTypeRepo;
        private ICountryRepo _countryRepo;
        private ICityRepo _cityRepo;
        private ICityAreaRepo _cityAreaRepo;
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private SQLFactory _connectionFactory;


        public IProductGroupRepo ProductGroupRepo { get { return _productGroupRepo; } }
        public ISubGroupRepo SubGroupRepo { get { return _subGroupRepo; } }
        public IItemRepo ItemRepo { get { return _itemRepo; } }
        public ICustomerTypeRepo CustomerTypeRepo { get { return _customerTypeRepo; } }
        public ICountryRepo CountryRepo { get { return _countryRepo; } }
        public ICityRepo CityRepo { get { return _cityRepo; } }
        public ICityAreaRepo CityAreaRepo { get { return _cityAreaRepo; } }
        bool _disposed = false;

        public void SaveChanges(bool createFollowUpTransaction = true)
        {
            try
            {
                _transaction.Commit();
                if (createFollowUpTransaction)
                    _transaction = _dbConnection.BeginTransaction();
            }
            catch (Exception)
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
            if (!_disposed)
            {
                _dbConnection.Dispose();
                _transaction.Dispose();
                _disposed = true;
            }
        }
    }
}