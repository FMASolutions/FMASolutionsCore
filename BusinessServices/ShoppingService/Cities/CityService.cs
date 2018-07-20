using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class CityService : ICityService
    {
        public CityService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _countryService = new CountryService(connectionString, dbType);
        }
        private IUnitOfWork _uow;
        ICountryService _countryService;

        #region ICityService
        public City GetByID(int id)
        {
            try
            {
                CityEntity entity = _uow.CityRepo.GetByID(id);
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
        public City GetByCode(string code)
        {
            try
            {
                CityEntity entity = _uow.CityRepo.GetByCode(code);
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
        public bool CreateNew(City model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.CityID = _uow.CityRepo.GetNextAvailableID();
                    CityEntity entity = ConvertModelToEntity(model);
                    success = _uow.CityRepo.Create(entity);
                    if (success)
                    {
                        model.CityID = entity.CityID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new City");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.InnerException.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<City> GetAll()
        {
            List<City> returnList = null;
            var initialList = _uow.CityRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<City>();
                foreach (var item in initialList)
                    returnList.Add(new City(new CustomModelState(), item.CityID, item.CityCode, item.CountryID, item.CityName));
            }
            return returnList;
        }

        public List<Country> GetAvailableCountries()
        {
            return _countryService.GetAll();
        }
        public bool UpdateDB(City newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.CityRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private City ConvertEntityToModel(CityEntity entityToConvert)
        {
            return new City(new CustomModelState()
                , entityToConvert.CityID
                , entityToConvert.CityCode
                , entityToConvert.CountryID
                , entityToConvert.CityName                
            );
        }
        private CityEntity ConvertModelToEntity(City modelToConvert)
        {
            return new CityEntity(modelToConvert.CityID
                , modelToConvert.CountryID
                , modelToConvert.CityCode
                , modelToConvert.CityName                
            );
        }
        private bool CodeExists(string code)
        {
            CityEntity entity = _uow.CityRepo.GetByCode(code);
            if (entity != null && entity.CityID > 0)
                return true;
            else
                return false;
        }
        private bool CountryIDExists(int id)
        {
            CountryEntity entity = _uow.CountryRepo.GetByID(id);
            if (entity != null && entity.CountryID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(City model)
        {
            if (model.ModelState.IsValid)
            {
                if (model.CityCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (string.IsNullOrEmpty(model.CityCode) || string.IsNullOrEmpty(model.CityName) ||model.CountryID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (CodeExists(model.CityCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (CountryIDExists(model.CountryID) == false)
                {
                    model.ModelState.AddError("Country ID", "Country ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(City newModel)
        {
            CityEntity idSearchResult = _uow.CityRepo.GetByID(newModel.CityID);
            CityEntity codeSearchResult = _uow.CityRepo.GetByCode(newModel.CityCode);
            CountryEntity countrySearchResult = _uow.CountryRepo.GetByID(newModel.CountryID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (codeSearchResult != null && codeSearchResult.CityID != newModel.CityID)
            {
                newModel.ModelState.AddError("CodeExists", "City Code already exists under a different ID (ID = " + codeSearchResult.CityID.ToString() + "Name: " + codeSearchResult.CityName + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || countrySearchResult == null || countrySearchResult.CountryID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (idSearchResult.CityCode != newModel.CityCode || idSearchResult.CountryID != newModel.CountryID || idSearchResult.CityName != newModel.CityName)
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
        private bool ValidateAllValues(City model)
        {
            if (ValidateID(model.CityID) == false || ValidateID(model.CountryID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.CityCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.CityCode) || string.IsNullOrEmpty(model.CityName))
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