using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class PostCodeService : IPostCodeService
    {
        public PostCodeService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _cityService = new CityService(connectionString, dbType);
        }
        private IUnitOfWork _uow;
        ICityService _cityService;

        #region IPostCodeService
        public PostCode GetByID(int id)
        {
            try
            {
                PostCodeEntity entity = _uow.PostCodeRepo.GetByID(id);
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
        public PostCode GetByCode(string code)
        {
            try
            {
                PostCodeEntity entity = _uow.PostCodeRepo.GetByCode(code);
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
        public bool CreateNew(PostCode model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.PostCodeID = _uow.PostCodeRepo.GetNextAvailableID();
                    PostCodeEntity entity = ConvertModelToEntity(model);
                    success = _uow.PostCodeRepo.Create(entity);
                    if (success)
                    {
                        model.PostCodeID = entity.PostCodeID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new Post Code");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.InnerException.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<PostCode> GetAll()
        {
            List<PostCode> returnList = null;
            var initialList = _uow.PostCodeRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<PostCode>();
                foreach (var item in initialList)
                    returnList.Add(new PostCode(new CustomModelState(),item.PostCodeID,item.PostCodeCode,item.CityID,item.PostCodeValue));
            }
            return returnList;
        }

        public List<City> GetAvailableCities()
        {
            return _cityService.GetAll();
        }
        public bool UpdateDB(PostCode newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.PostCodeRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private PostCode ConvertEntityToModel(PostCodeEntity entityToConvert)
        {
            return new PostCode(new CustomModelState()
                ,entityToConvert.PostCodeID
                ,entityToConvert.PostCodeCode
                ,entityToConvert.CityID
                ,entityToConvert.PostCodeValue
            );
        }
        private PostCodeEntity ConvertModelToEntity(PostCode modelToConvert)
        {
            return new PostCodeEntity(modelToConvert.PostCodeID,
                modelToConvert.CityID
                ,modelToConvert.PostCodeCode
                ,modelToConvert.PostCodeValue
            );            
        }
        private bool CodeExists(string code)
        {
            PostCodeEntity entity = _uow.PostCodeRepo.GetByCode(code);
            if (entity != null && entity.PostCodeID > 0)
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
        private bool ValidateForCreate(PostCode model)
        {
            if (model.ModelState.IsValid)
            {
                if (model.PostCodeCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (string.IsNullOrEmpty(model.PostCodeCode) || string.IsNullOrEmpty(model.PostCodeValue) ||model.CityID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (CodeExists(model.PostCodeCode))
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
        private bool ValidateForUpdate(PostCode newModel)
        {
            PostCodeEntity idSearchResult = _uow.PostCodeRepo.GetByID(newModel.PostCodeID);
            PostCodeEntity codeSearchResult = _uow.PostCodeRepo.GetByCode(newModel.PostCodeCode);
            CityEntity citySearchResult = _uow.CityRepo.GetByID(newModel.CityID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (codeSearchResult != null && codeSearchResult.PostCodeID != newModel.PostCodeID)
            {
                newModel.ModelState.AddError("CodeExists", "Post Code Code already exists under a different ID (ID = " + codeSearchResult.PostCodeID.ToString() + "Name: " + codeSearchResult.PostCodeValue + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || citySearchResult == null || citySearchResult.CityID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (idSearchResult.PostCodeCode != newModel.PostCodeCode || idSearchResult.CityID != newModel.CityID || idSearchResult.PostCodeValue != newModel.PostCodeValue)
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
        private bool ValidateAllValues(PostCode model)
        {
            if (ValidateID(model.PostCodeID) == false || ValidateID(model.CityID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.PostCodeCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.PostCodeCode) || string.IsNullOrEmpty(model.PostCodeValue))
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