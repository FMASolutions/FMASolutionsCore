using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _customerTypeService = new CustomerTypeService(connectionString, dbType);
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _uow.Dispose();
                _customerTypeService.Dispose();
            }
        }
        private bool _disposing = false;
        private IUnitOfWork _uow;
        ICustomerTypeService _customerTypeService;

        #region ICustomerService
        public Customer GetByID(int id)
        {
            try
            {
                CustomerEntity entity = _uow.CustomerRepo.GetByID(id);
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
        public Customer GetByCode(string code)
        {
            try
            {
                CustomerEntity entity = _uow.CustomerRepo.GetByCode(code);
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
        public bool CreateNew(Customer model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.CustomerID = _uow.CustomerRepo.GetNextAvailableID();
                    CustomerEntity entity = ConvertModelToEntity(model);
                    success = _uow.CustomerRepo.Create(entity);
                    if (success)
                    {
                        model.CustomerID = entity.CustomerID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Customer");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<Customer> GetAll()
        {
            List<Customer> returnList = null;
            var initialList = _uow.CustomerRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<Customer>();
                foreach (var item in initialList)
                    returnList.Add(new Customer(new CustomModelState(), item.CustomerID, item.CustomerCode, item.CustomerTypeID, item.CustomerName, item.CustomerContactNumber, item.CustomerEmailAddress));
            }
            return returnList;
        }

        public List<CustomerType> GetAvailableCustomerTypes()
        {
            return _customerTypeService.GetAll();
        }
        public bool UpdateDB(Customer newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CustomerRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private Customer ConvertEntityToModel(CustomerEntity entityToConvert)
        {
            return new Customer(new CustomModelState()
                , entityToConvert.CustomerID
                , entityToConvert.CustomerCode
                , entityToConvert.CustomerTypeID
                , entityToConvert.CustomerName
                , entityToConvert.CustomerContactNumber
                , entityToConvert.CustomerEmailAddress              

            );
        }
        private CustomerEntity ConvertModelToEntity(Customer modelToConvert)
        {
            return new CustomerEntity(modelToConvert.CustomerID, modelToConvert.CustomerTypeID, modelToConvert.CustomerCode, modelToConvert.CustomerName, modelToConvert.CustomerContactNumber, modelToConvert.CustomerEmailAddress);
        }
        private bool CodeExists(string code)
        {
            CustomerEntity entity = _uow.CustomerRepo.GetByCode(code);
            if (entity != null && entity.CustomerID > 0)
                return true;
            else
                return false;
        }
        private bool CustomerTypeIDExists(int id)
        {
            CustomerTypeEntity entity = _uow.CustomerTypeRepo.GetByID(id);
            if (entity != null && entity.CustomerTypeID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(Customer model)
        {
            if (model.ModelState.IsValid)
            {
                
                if (string.IsNullOrEmpty(model.CustomerCode) || string.IsNullOrEmpty(model.CustomerName) || string.IsNullOrEmpty(model.CustomerContactNumber) || string.IsNullOrEmpty(model.CustomerName) || model.CustomerTypeID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.CustomerCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (CodeExists(model.CustomerCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (CustomerTypeIDExists(model.CustomerTypeID) == false)
                {
                    model.ModelState.AddError("CustomerTypeID", "Customer Type ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(Customer newModel)
        {
            CustomerEntity sgIDSearchResult = _uow.CustomerRepo.GetByID(newModel.CustomerID);
            CustomerEntity sgCodeSearchResult = _uow.CustomerRepo.GetByCode(newModel.CustomerCode);
            CustomerTypeEntity pgIDSearchResult = _uow.CustomerTypeRepo.GetByID(newModel.CustomerTypeID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (sgCodeSearchResult != null && sgCodeSearchResult.CustomerID != newModel.CustomerID)
            {
                newModel.ModelState.AddError("CodeExists", "Customer Code already exists under a different ID (ID = " + sgCodeSearchResult.CustomerID.ToString() + "Name: " + sgCodeSearchResult.CustomerName + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || pgIDSearchResult == null || pgIDSearchResult.CustomerTypeID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (sgIDSearchResult.CustomerCode != newModel.CustomerCode || sgIDSearchResult.CustomerTypeID != newModel.CustomerTypeID || sgIDSearchResult.CustomerName != newModel.CustomerName || sgIDSearchResult.CustomerContactNumber != newModel.CustomerContactNumber || sgIDSearchResult.CustomerEmailAddress != newModel.CustomerEmailAddress)
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
            if (code.Length > 5) return false;
            else return true;
        }
        private bool ValidateAllValues(Customer model)
        {
            if (ValidateID(model.CustomerID) == false || ValidateID(model.CustomerTypeID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CustomerCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CustomerCode) || string.IsNullOrEmpty(model.CustomerName) || string.IsNullOrEmpty(model.CustomerContactNumber) || string.IsNullOrEmpty(model.CustomerEmailAddress))
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