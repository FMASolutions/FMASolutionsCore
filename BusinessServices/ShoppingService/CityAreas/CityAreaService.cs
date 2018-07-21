using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CityAreaService : ICityAreaService
    {
        public CityAreaService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _cityService = new CityService(connectionString, dbType);
        }
        private IUnitOfWork _uow;
        ICityService _cityService;

        #region ICityService
        public CityArea GetByID(int id)
        {
            try
            {
                CityAreaEntity entity = _uow.CityAreaRepo.GetByID(id);
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
        public CityArea GetByCode(string code)
        {
            try
            {
                CityAreaEntity entity = _uow.CityAreaRepo.GetByCode(code);
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
        public bool CreateNew(CityArea model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.CityAreaID = _uow.CityAreaRepo.GetNextAvailableID();
                    CityAreaEntity entity = ConvertModelToEntity(model);
                    success = _uow.CityAreaRepo.Create(entity);
                    if (success)
                    {
                        model.CityAreaID = entity.CityAreaID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new City Area");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.InnerException.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<CityArea> GetAll()
        {
            List<CityArea> returnList = null;
            var initialList = _uow.CityAreaRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<CityArea>();
                foreach (var item in initialList)
                    returnList.Add(new CityArea(new CustomModelState(),item.CityAreaID,item.CityAreaCode,item.CityID,item.CityAreaName));
            }
            return returnList;
        }

        public List<City> GetAvailableCities()
        {
            return _cityService.GetAll();
        }
        public bool UpdateDB(CityArea newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CityAreaRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private CityArea ConvertEntityToModel(CityAreaEntity entityToConvert)
        {
            return new CityArea(new CustomModelState()
                ,entityToConvert.CityAreaID
                ,entityToConvert.CityAreaCode
                ,entityToConvert.CityID
                ,entityToConvert.CityAreaName
            );
        }
        private CityAreaEntity ConvertModelToEntity(CityArea modelToConvert)
        {
            return new CityAreaEntity(modelToConvert.CityAreaID,
                modelToConvert.CityID
                ,modelToConvert.CityAreaCode
                ,modelToConvert.CityAreaName
            );            
        }
        private bool CodeExists(string code)
        {
            CityAreaEntity entity = _uow.CityAreaRepo.GetByCode(code);
            if (entity != null && entity.CityAreaID > 0)
                return true;
            else
                return false;
        }
        private bool CityIDExists(int id)
        {
            CityEntity entity = _uow.CityRepo.GetByID(id);
            if (entity != null && entity.CityID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(CityArea model)
        {
            if (model.ModelState.IsValid)
            {
                if (model.CityAreaCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (string.IsNullOrEmpty(model.CityAreaCode) || string.IsNullOrEmpty(model.CityAreaName) ||model.CityID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (CodeExists(model.CityAreaCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (CityIDExists(model.CityID) == false)
                {
                    model.ModelState.AddError("City ID", "City ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(CityArea newModel)
        {
            CityAreaEntity idSearchResult = _uow.CityAreaRepo.GetByID(newModel.CityAreaID);
            CityAreaEntity codeSearchResult = _uow.CityAreaRepo.GetByCode(newModel.CityAreaCode);
            CityEntity citySearchResult = _uow.CityRepo.GetByID(newModel.CityID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (codeSearchResult != null && codeSearchResult.CityAreaID != newModel.CityAreaID)
            {
                newModel.ModelState.AddError("CodeExists", "City Area Code already exists under a different ID (ID = " + codeSearchResult.CityAreaID.ToString() + "Name: " + codeSearchResult.CityAreaName + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || citySearchResult == null || citySearchResult.CityID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (idSearchResult.CityAreaCode != newModel.CityAreaCode || idSearchResult.CityID != newModel.CityID || idSearchResult.CityAreaName != newModel.CityAreaName)
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
        private bool ValidateAllValues(CityArea model)
        {
            if (ValidateID(model.CityAreaID) == false || ValidateID(model.CityID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CityAreaCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CityAreaCode) || string.IsNullOrEmpty(model.CityAreaName))
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