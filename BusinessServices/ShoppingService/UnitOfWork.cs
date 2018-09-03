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

            //When adding a new Repository Always remember to add it to the private "Reset" method @ the bottom
            _productGroupRepo = new ProductGroupRepo(_transaction);
            _subGroupRepo = new SubGroupRepo(_transaction);
            _itemRepo = new ItemRepo(_transaction);
            _customerTypeRepo = new CustomerTypeRepo(_transaction);
            _countryRepo = new CountryRepo(_transaction);
            _cityRepo = new CityRepo(_transaction);
            _cityAreaRepo = new CityAreaRepo(_transaction);            
            _addressLocationRepo = new AddressLocationRepo(_transaction);
            _customerRepo = new CustomerRepo(_transaction);
            _customerAddressRepo = new CustomerAddressRepo(_transaction);
        }
        ~UnitOfWork()
        {
            dispose(false);
        }
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private SQLFactory _connectionFactory;

        public IProductGroupRepo ProductGroupRepo { get { return _productGroupRepo ?? (_productGroupRepo = new ProductGroupRepo(_transaction)); } }
        public ISubGroupRepo SubGroupRepo { get { return _subGroupRepo ?? (_subGroupRepo = new SubGroupRepo(_transaction)); } }
        public IItemRepo ItemRepo { get { return _itemRepo ?? (_itemRepo = new ItemRepo(_transaction)); } }
        public ICustomerTypeRepo CustomerTypeRepo { get { return _customerTypeRepo ?? (_customerTypeRepo = new CustomerTypeRepo(_transaction)); } }
        public ICountryRepo CountryRepo { get { return _countryRepo ?? (_countryRepo = new CountryRepo(_transaction)); } }
        public ICityRepo CityRepo { get { return _cityRepo ?? (_cityRepo = new CityRepo(_transaction)); } }
        public ICityAreaRepo CityAreaRepo { get { return _cityAreaRepo ?? (_cityAreaRepo = new CityAreaRepo(_transaction)); } }        
        public IAddressLocationRepo AddressLocationRepo { get { return _addressLocationRepo ?? (_addressLocationRepo = new AddressLocationRepo(_transaction)); } }
        public ICustomerRepo CustomerRepo { get { return _customerRepo ?? (_customerRepo = new CustomerRepo(_transaction)); } }
        public ICustomerAddressRepo CustomerAddressRepo { get { return _customerAddressRepo ?? (_customerAddressRepo = new CustomerAddressRepo(_transaction)); } }
        bool _disposing = false;

        public void SaveChanges()
        {
            try
            {
                _transaction.Commit();
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
                ResetRepos();
            }
        }
        public void RollBack(bool createFollowUpTransaction = true)
        {
            _transaction.Rollback();
            if (createFollowUpTransaction)
                _transaction = _dbConnection.BeginTransaction();
        }
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposing)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_dbConnection != null)
                    {
                        _dbConnection.Dispose();
                        _dbConnection = null;
                    }
                }
                _disposing = true;
            }
        }


        private void ResetRepos()
        {
            _productGroupRepo = null;
            _subGroupRepo = null;
            _itemRepo = null;
            _customerTypeRepo = null;
            _countryRepo = null;
            _cityRepo = null;
            _cityAreaRepo = null;            
            _addressLocationRepo = null;
            _customerRepo = null;
            _customerAddressRepo = null;
        }
        private IProductGroupRepo _productGroupRepo;
        private ISubGroupRepo _subGroupRepo;
        private IItemRepo _itemRepo;
        private ICustomerTypeRepo _customerTypeRepo;
        private ICountryRepo _countryRepo;
        private ICityRepo _cityRepo;
        private ICityAreaRepo _cityAreaRepo;        
        private IAddressLocationRepo _addressLocationRepo;
        private ICustomerRepo _customerRepo;
        private ICustomerAddressRepo _customerAddressRepo;
    }
}