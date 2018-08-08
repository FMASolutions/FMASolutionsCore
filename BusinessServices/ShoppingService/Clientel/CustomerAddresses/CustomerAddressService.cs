using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CustomerAddressService : ICustomerAddressService
    {
        public CustomerAddressService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _customerService = new CustomerService(connectionString, dbType);
            _addressLocationService = new AddressLocationService(connectionString, dbType);
        }
        public void Dispose()
        {
            if (!_disposing)
            {
                _disposing = true;
                _customerService.Dispose();
                _addressLocationService.Dispose();
                _uow.Dispose();
            }
        }

        private bool _disposing = false;
        private IUnitOfWork _uow;
        ICustomerService _customerService;
        IAddressLocationService _addressLocationService;

        #region ICustomerAddressService
        public CustomerAddress GetByID(int id)
        {
            try
            {
                CustomerAddressEntity entity = _uow.CustomerAddressRepo.GetByID(id);
                if (entity != null)
                    return ConvertEntityToModel(entity);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public CustomerAddress GetByCode(string code)
        {
            try
            {
                CustomerAddressEntity entity = _uow.CustomerAddressRepo.GetByCode(code);
                if (entity != null)
                    return ConvertEntityToModel(entity);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool CreateNew(CustomerAddress model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    CustomerAddressEntity entity = ConvertModelToEntity(model);
                    success = _uow.CustomerAddressRepo.Create(entity);
                    if (success)
                    {
                        CustomerAddress createdModel = GetByCode(model.CustomerAddressCode);
                        model.CustomerAddressID = createdModel.CustomerAddressID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Customer Address: ");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<CustomerAddress> GetAll()
        {
            List<CustomerAddress> returnList = null;
            var initialList = _uow.CustomerAddressRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<CustomerAddress>();
                foreach (var item in initialList)
                    returnList.Add(new CustomerAddress(new CustomModelState(),item.CustomerAddressID,item.CustomerAddressCode,item.CustomerID,item.AddressLocationID,item.IsDefaultAddress,item.CustomerAddressDescription));
            }
            return returnList;
        }

        public List<Customer> GetAvailableCustomers()
        {
            return _customerService.GetAll();
        }
        public List<AddressLocation> GetAvailableAddressLocations()
        {
            return _addressLocationService.GetAll();
        }
        public bool UpdateDB(CustomerAddress newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CustomerAddressRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private CustomerAddress ConvertEntityToModel(CustomerAddressEntity entityToConvert)
        {
            return new CustomerAddress(new CustomModelState()
                , entityToConvert.CustomerAddressID
                , entityToConvert.CustomerAddressCode
                , entityToConvert.CustomerID
                , entityToConvert.AddressLocationID
                , entityToConvert.IsDefaultAddress
                , entityToConvert.CustomerAddressDescription
            );
        }
        private CustomerAddressEntity ConvertModelToEntity(CustomerAddress modelToConvert)
        {
            return new CustomerAddressEntity(modelToConvert.CustomerAddressID
                , modelToConvert.CustomerID
                , modelToConvert.AddressLocationID
                , modelToConvert.CustomerAddressCode
                , modelToConvert.CustomerAddressDescription
                , modelToConvert.IsDefaultAddress
            );
        }
        private bool CodeExists(string code)
        {
            CustomerAddressEntity entity = _uow.CustomerAddressRepo.GetByCode(code);
            if (entity != null && entity.CustomerAddressID > 0)
                return true;
            else
                return false;
        }
        private bool CustomerIDExists(int id)
        {
            CustomerEntity entity = _uow.CustomerRepo.GetByID(id);
            if (entity != null && entity.CustomerID > 0)
                return true;
            else
                return false;
        }
        private bool AddressLocationIDExists(int id)
        {
            AddressLocationEntity entity = _uow.AddressLocationRepo.GetByID(id);
            if (entity != null && entity.AddressLocationID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(CustomerAddress model)
        {
            if (model.ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.CustomerAddressCode) || string.IsNullOrEmpty(model.CustomerAddressDescription)  || model.AddressLocationID <= 0 || model.CustomerID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.CustomerAddressCode.Length > 7)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 7 characters");
                    return false;
                }

                else if (CodeExists(model.CustomerAddressCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (CustomerIDExists(model.CustomerID) == false)
                {
                    model.ModelState.AddError("CustomerID", "Customer ID Provided does not exists");
                    return false;
                }
                else if (AddressLocationIDExists(model.AddressLocationID) == false)
                {
                    model.ModelState.AddError("AddressLocationID", "Address Location ID Provided does not exist");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(CustomerAddress newModel)
        {
            CustomerAddressEntity sgIDSearchResult = _uow.CustomerAddressRepo.GetByID(newModel.CustomerAddressID);
            CustomerAddressEntity sgCodeSearchResult = _uow.CustomerAddressRepo.GetByCode(newModel.CustomerAddressCode);
            CustomerEntity custIDSearchResult = _uow.CustomerRepo.GetByID(newModel.CustomerID);
            AddressLocationEntity addressIDSearchResult = _uow.AddressLocationRepo.GetByID(newModel.AddressLocationID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (sgCodeSearchResult != null && sgCodeSearchResult.CustomerAddressID != newModel.CustomerAddressID)
            {
                newModel.ModelState.AddError("CodeExists", "Customer Address Code already exists under a different ID (ID = " + sgCodeSearchResult.CustomerAddressID.ToString() + "Description: " + sgCodeSearchResult.CustomerAddressDescription + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || custIDSearchResult == null || custIDSearchResult.CustomerID <= 0 || addressIDSearchResult == null || addressIDSearchResult.AddressLocationID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (sgIDSearchResult.CustomerAddressCode != newModel.CustomerAddressCode || sgIDSearchResult.AddressLocationID != newModel.AddressLocationID || sgIDSearchResult.CustomerID != newModel.CustomerID || sgIDSearchResult.CustomerAddressDescription != newModel.CustomerAddressDescription || sgIDSearchResult.IsDefaultAddress != newModel.IsDefaultAddress)
                return true;
            else
            {
                newModel.ModelState.AddError("NoChanges", "No Changes detected");
                return false;
            }
        }

        private bool ValidateID(int id)
        {
            if (id > 0 && id < 9999999) return true;
            else return false;
        }
        private bool ValidateCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            if (string.IsNullOrWhiteSpace(code)) return false;
            if (code.Length > 7) return false;
            else return true;
        }
        private bool ValidateAllValues(CustomerAddress model)
        {
            if (ValidateID(model.CustomerAddressID) == false || ValidateID(model.CustomerID) == false || ValidateID(model.AddressLocationID))
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CustomerAddressCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 7 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CustomerAddressCode) || string.IsNullOrEmpty(model.CustomerAddressDescription))
            {
                model.ModelState.AddError("Null", "Values cant be blank.");
                return false;
            }
            else
                return true;
        }
        #endregion
    }
}