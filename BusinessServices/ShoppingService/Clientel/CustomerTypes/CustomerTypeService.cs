using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CustomerTypeService : ICustomerTypeService
    {
        public CustomerTypeService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _uow.Dispose();
            }
        }
        private bool _disposing = false;
        private IUnitOfWork _uow;
        public CustomerType GetByID(int id)
        {
            try
            {
                CustomerTypeEntity entity = _uow.CustomerTypeRepo.GetByID(id);
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
        public CustomerType GetByCode(string code)
        {
            try
            {
                CustomerTypeEntity entity = _uow.CustomerTypeRepo.GetByCode(code);
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
        public bool CreateNew(CustomerType model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.CustomerTypeID = _uow.CustomerTypeRepo.GetNextAvailableID();
                    CustomerTypeEntity entity = ConvertModelToEntity(model);
                    success = _uow.CustomerTypeRepo.Create(entity);
                    if (success)
                    {
                        model.CustomerTypeID = entity.CustomerTypeID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Customer Type");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<CustomerType> GetAll()
        {
            List<CustomerType> returnList = null;
            var initialList = _uow.CustomerTypeRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<CustomerType>();
                foreach (var item in initialList)
                    returnList.Add(new CustomerType(new CustomModelState(), item.CustomerTypeID, item.CustomerTypeCode, item.CustomerTypeName));
            }
            return returnList;
        }
        public bool UpdateDB(CustomerType newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CustomerTypeRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }

        #region Private Functions
        private bool CodeExists(string code)
        {
            CustomerTypeEntity ent = _uow.CustomerTypeRepo.GetByCode(code);
            if (ent != null && ent.CustomerTypeID > 0)
                return true;
            else
                return false;
        }
        private CustomerType ConvertEntityToModel(CustomerTypeEntity entity)
        {
            return new CustomerType(new CustomModelState()
                , entity.CustomerTypeID
                , entity.CustomerTypeCode
                , entity.CustomerTypeName
            );
        }
        private CustomerTypeEntity ConvertModelToEntity(CustomerType model)
        {
            CustomerTypeEntity returnEntity = new CustomerTypeEntity();
            returnEntity.CustomerTypeID = model.CustomerTypeID;
            returnEntity.CustomerTypeCode = model.CustomerTypeCode;
            returnEntity.CustomerTypeName = model.CustomerTypeName;
            return returnEntity;
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
        private bool ValidateAllValues(CustomerType model)
        {
            if (ValidateID(model.CustomerTypeID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CustomerTypeCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CustomerTypeCode) || string.IsNullOrEmpty(model.CustomerTypeName))
            {
                model.ModelState.AddError("Null", "Values cant be blank.");
                return false;
            }
            else return true;
        }
        private bool ValidateForUpdate(CustomerType newModel)
        {
            CustomerTypeEntity entityFromIDSearch = _uow.CustomerTypeRepo.GetByID(newModel.CustomerTypeID);
            CustomerTypeEntity entityFromCodeSearch = _uow.CustomerTypeRepo.GetByCode(newModel.CustomerTypeCode);
            //Check our new code (if it is even new) doesn't exist under a different ID which would cause a proble with the UNIQUE contraint on the CODE column.                        
            if (entityFromCodeSearch != null && entityFromCodeSearch.ID != newModel.CustomerTypeID)
            {
                newModel.ModelState.AddError("CodeExists", @"Customer Type Code already exists under a different ID (ID = " + entityFromCodeSearch.CustomerTypeID.ToString() + " Name = " + entityFromCodeSearch.CustomerTypeName + ") and must be unique");
                return false;
            }
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //If user changed something, return true at this point
            else if (entityFromIDSearch.CustomerTypeName != newModel.CustomerTypeName || entityFromIDSearch.CustomerTypeCode != newModel.CustomerTypeCode)
                return true;
            else
                newModel.ModelState.AddError("NoChange", "No Changes detected");
            return false;
        }
        private bool ValidateForCreate(CustomerType model)
        {
            if (model.ModelState.IsValid)
            {                
                if (string.IsNullOrEmpty(model.CustomerTypeCode) || string.IsNullOrEmpty(model.CustomerTypeName))
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.CustomerTypeCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (CodeExists(model.CustomerTypeCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
            }
            else
                return false;
            return true;
        }
        #endregion
    }
}