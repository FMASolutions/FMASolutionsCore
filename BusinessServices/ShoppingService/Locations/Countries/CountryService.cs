using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CountryService : ICountryService
    {
        public CountryService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
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
         public Country GetByID(int id)
        {
            try
            {
                CountryEntity entity = _uow.CountryRepo.GetByID(id);
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
        public Country GetByCode(string code)
        {
            try
            {
                CountryEntity entity = _uow.CountryRepo.GetByCode(code);
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
        public bool CreateNew(Country model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    CountryEntity entity = ConvertModelToEntity(model);
                    success = _uow.CountryRepo.Create(entity);
                    if (success)
                    {
                        Country createdModel = GetByCode(model.CountryCode);
                        model.CountryID = createdModel.CountryID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Country");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<Country> GetAll()
        {
            List<Country> returnList = null;
            var initialList = _uow.CountryRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<Country>();
                foreach (var item in initialList)
                    returnList.Add(new Country(new CustomModelState(), item.CountryID, item.CountryCode, item.CountryName));
            }
            return returnList;
        }
        public bool UpdateDB(Country newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CountryRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }

        #region Private Functions
        private bool CodeExists(string code)
        {
            CountryEntity ent = _uow.CountryRepo.GetByCode(code);
            if (ent != null && ent.CountryID > 0)
                return true;
            else
                return false;
        }
        private Country ConvertEntityToModel(CountryEntity entity)
        {
            return new Country(new CustomModelState()
                , entity.CountryID
                , entity.CountryCode
                , entity.CountryName
            );
        }
        private CountryEntity ConvertModelToEntity(Country model)
        {
            CountryEntity returnEntity = new CountryEntity();
            returnEntity.CountryID = model.CountryID;
            returnEntity.CountryCode = model.CountryCode;
            returnEntity.CountryName = model.CountryName;
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
            if (code.Length > 7) return false;
            else return true;
        }
        private bool ValidateAllValues(Country model)
        {
            if (ValidateID(model.CountryID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CountryCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 7 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CountryCode) || string.IsNullOrEmpty(model.CountryName))
            {
                model.ModelState.AddError("Null", "Values cant be blank.");
                return false;
            }
            else return true;
        }
        private bool ValidateForUpdate(Country newModel)
        {
            CountryEntity entityFromIDSearch = _uow.CountryRepo.GetByID(newModel.CountryID);
            CountryEntity entityFromCodeSearch = _uow.CountryRepo.GetByCode(newModel.CountryCode);
            //Check our new code (if it is even new) doesn't exist under a different ID which would cause a proble with the UNIQUE contraint on the CODE column.                        
            if (entityFromCodeSearch != null && entityFromCodeSearch.ID != newModel.CountryID)
            {
                newModel.ModelState.AddError("CodeExists", @"Country Code already exists under a different ID (ID = " + entityFromCodeSearch.CountryID.ToString() + " Name = " + entityFromCodeSearch.CountryName + ") and must be unique");
                return false;
            }
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //If user changed something, return true at this point
            else if (entityFromIDSearch.CountryName != newModel.CountryName || entityFromIDSearch.CountryCode != newModel.CountryCode)
                return true;
            else
                newModel.ModelState.AddError("NoChange", "No Changes detected");
            return false;
        }
        private bool ValidateForCreate(Country model)
        {
            if (model.ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.CountryCode) || string.IsNullOrEmpty(model.CountryName))
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.CountryCode.Length > 7)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 7 characters");
                    return false;
                } 
                else if (CodeExists(model.CountryCode))
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