using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class AddressLocationService : IAddressLocationService
    {
        public AddressLocationService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _connectionSTring = connectionString;
            _dbType = dbType;
            _uow = new UnitOfWork(connectionString, dbType);
            _cityAreaService = new CityAreaService(connectionString, dbType);            
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _cityAreaService.Dispose();                
                _uow.Dispose();
            }
        }
        private bool _disposing = false;
        private IUnitOfWork _uow;
        ICityAreaService _cityAreaService;        
        private string _connectionSTring;
        private SQLAppConfigTypes.SQLAppConfigTypes _dbType;


        #region ICityService
        public AddressLocation GetByID(int id)
        {
            try
            {
                AddressLocationEntity entity = _uow.AddressLocationRepo.GetByID(id);
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
        public AddressLocation GetByCode(string code)
        {
            try
            {
                AddressLocationEntity entity = _uow.AddressLocationRepo.GetByCode(code);
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
        public bool CreateNew(AddressLocation model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    AddressLocationEntity entity = ConvertModelToEntity(model);
                    success = _uow.AddressLocationRepo.Create(entity);
                    if (success)
                    {
                        AddressLocation createdModel = GetByCode(model.AddressLocationCode);
                        model.AddressLocationID = createdModel.AddressLocationID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Address Location");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<AddressLocation> GetAll()
        {
            List<AddressLocation> returnList = null;
            var initialList = _uow.AddressLocationRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<AddressLocation>();
                foreach (var item in initialList)
                    returnList.Add(new AddressLocation(new CustomModelState(), item.AddressLocationID, item.AddressLocationCode, item.CityAreaID, item.AddressLine1, item.AddressLine2, item.PostCode));
            }
            return returnList;
        }

        public List<CityArea> GetAvailableCityAreas()
        {
            return _cityAreaService.GetAll();
        }   
        public bool UpdateDB(AddressLocation newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.AddressLocationRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private AddressLocation ConvertEntityToModel(AddressLocationEntity entityToConvert)
        {
            return new AddressLocation(new CustomModelState()
                , entityToConvert.AddressLocationID
                , entityToConvert.AddressLocationCode
                , entityToConvert.CityAreaID                
                , entityToConvert.AddressLine1
                , entityToConvert.AddressLine2
                , entityToConvert.PostCode
            );
        }
        private AddressLocationEntity ConvertModelToEntity(AddressLocation modelToConvert)
        {
            return new AddressLocationEntity(modelToConvert.AddressLocationID
                , modelToConvert.CityAreaID                
                , modelToConvert.AddressLocationCode
                , modelToConvert.AddressLine1
                , modelToConvert.AddressLine2
                , modelToConvert.PostCode
            );
        }
        private bool CodeExists(string code)
        {
            AddressLocationEntity entity = _uow.AddressLocationRepo.GetByCode(code);
            if (entity != null && entity.AddressLocationID > 0)
                return true;
            else
                return false;
        }
        private bool CityAreaIDExists(int id)
        {
            CityAreaEntity entity = _uow.CityAreaRepo.GetByID(id);
            if (entity != null && entity.CityAreaID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(AddressLocation model)
        {
            if (model.ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.AddressLocationCode) || string.IsNullOrEmpty(model.PostCode) || string.IsNullOrEmpty(model.AddressLine1) || string.IsNullOrEmpty(model.AddressLine2) || model.CityAreaID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.AddressLocationCode.Length > 7)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 7 characters");
                    return false;
                }
                else if (model.PostCode.Length > 10)
                {
                    model.ModelState.AddError("PostCodeLength","Postcode should not be more than 10 characters");
                    return false;
                }                
                else if (CodeExists(model.AddressLocationCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (CityAreaIDExists(model.CityAreaID) == false)
                {
                    model.ModelState.AddError("City Area ID", "City Area ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(AddressLocation newModel)
        {
            AddressLocationEntity idSearchResult = _uow.AddressLocationRepo.GetByID(newModel.AddressLocationID);
            AddressLocationEntity codeSearchResult = _uow.AddressLocationRepo.GetByCode(newModel.AddressLocationCode);
            CityAreaEntity cityAreaSearchResult = _uow.CityAreaRepo.GetByID(newModel.CityAreaID);            
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (codeSearchResult != null && codeSearchResult.AddressLocationID != newModel.AddressLocationID)
            {
                newModel.ModelState.AddError("CodeExists", "Address Location Code already exists under a different ID (ID = " + codeSearchResult.AddressLocationID.ToString() + "Code: " + codeSearchResult.AddressLocationCode + " Line1: " + codeSearchResult.AddressLine1 + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || cityAreaSearchResult == null || cityAreaSearchResult.CityAreaID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            else if (newModel.PostCode.Length > 10)
            {
                newModel.ModelState.AddError("PostCodeLength","Postcode should not be more than 10 characters");
                return false;
            }
            //Check something has actually changed
            else if (idSearchResult.AddressLocationCode != newModel.AddressLocationCode || idSearchResult.CityAreaID != newModel.CityAreaID || idSearchResult.AddressLine1 != newModel.AddressLine1 || idSearchResult.AddressLine2 != newModel.AddressLine2 || idSearchResult.PostCode != newModel.PostCode)
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
        private bool ValidateAllValues(AddressLocation model)
        {
            if (ValidateID(model.AddressLocationID) == false || ValidateID(model.CityAreaID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.AddressLocationCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 7 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.AddressLocationCode) || string.IsNullOrEmpty(model.AddressLine1) || string.IsNullOrEmpty(model.AddressLine2) || string.IsNullOrEmpty(model.PostCode))
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