using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class SubGroupService : ISubGroupService
    {
        public SubGroupService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _productGroupService = new ProductGroupService(connectionString, dbType);
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _productGroupService.Dispose();
                _uow.Dispose();
            }
        }

        private bool _disposing = false;
        private IUnitOfWork _uow;
        IProductGroupService _productGroupService;

        #region ISubGroupService
        public SubGroup GetByID(int id)
        {
            try
            {
                SubGroupEntity entity = _uow.SubGroupRepo.GetByID(id);
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
        public SubGroup GetByCode(string code)
        {
            try
            {
                SubGroupEntity entity = _uow.SubGroupRepo.GetByCode(code);
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
        public bool CreateNew(SubGroup model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.SubGroupID = _uow.SubGroupRepo.GetNextAvailableID();
                    SubGroupEntity entity = ConvertModelToEntity(model);
                    success = _uow.SubGroupRepo.Create(entity);
                    if (success)
                    {
                        model.SubGroupID = entity.SubGroupID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new sub group: ");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<SubGroup> GetAll()
        {
            List<SubGroup> returnList = null;
            var initialList = _uow.SubGroupRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<SubGroup>();
                foreach (var item in initialList)
                    returnList.Add(new SubGroup(new CustomModelState(), item.SubGroupID, item.SubGroupCode, item.ProductGroupID, item.SubGroupName, item.SubGroupDescription));
            }
            return returnList;
        }

        public List<ProductGroup> GetAvailableProductGroups()
        {
            return _productGroupService.GetAll();
        }
        public bool UpdateDB(SubGroup newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.SubGroupRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }
        #endregion

        #region Private Functions
        private SubGroup ConvertEntityToModel(SubGroupEntity entityToConvert)
        {
            return new SubGroup(new CustomModelState()
                , entityToConvert.SubGroupID
                , entityToConvert.SubGroupCode
                , entityToConvert.ProductGroupID
                , entityToConvert.SubGroupName
                , entityToConvert.SubGroupDescription
            );
        }
        private SubGroupEntity ConvertModelToEntity(SubGroup modelToConvert)
        {
            return new SubGroupEntity(modelToConvert.SubGroupID
                , modelToConvert.ProductGroupID
                , modelToConvert.SubGroupCode
                , modelToConvert.SubGroupName
                , modelToConvert.SubGroupDescription
            );
        }
        private bool CodeExists(string code)
        {
            SubGroupEntity entity = _uow.SubGroupRepo.GetByCode(code);
            if (entity != null && entity.SubGroupID > 0)
                return true;
            else
                return false;
        }
        private bool ProductGroupIDExists(int id)
        {
            ProductGroupEntity entity = _uow.ProductGroupRepo.GetByID(id);
            if (entity != null && entity.ProductGroupID > 0)
                return true;
            else
                return false;
        }
        private bool ValidateForCreate(SubGroup model)
        {
            if (model.ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.SubGroupCode) || string.IsNullOrEmpty(model.SubGroupName) || string.IsNullOrEmpty(model.SubGroupDescription) || model.ProductGroupID <= 0)
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (model.SubGroupCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                
                else if (CodeExists(model.SubGroupCode))
                {
                    model.ModelState.AddError("CodeExists", "The code provided already exists and must be unique.");
                    return false;
                }
                else if (ProductGroupIDExists(model.ProductGroupID) == false)
                {
                    model.ModelState.AddError("ProductGroupID", "Product Group ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;

        }
        private bool ValidateForUpdate(SubGroup newModel)
        {
            SubGroupEntity sgIDSearchResult = _uow.SubGroupRepo.GetByID(newModel.SubGroupID);
            SubGroupEntity sgCodeSearchResult = _uow.SubGroupRepo.GetByCode(newModel.SubGroupCode);
            ProductGroupEntity pgIDSearchResult = _uow.ProductGroupRepo.GetByID(newModel.ProductGroupID);
            //Ensure new code (is it's new) doesn't already exist under a different ID.
            if (sgCodeSearchResult != null && sgCodeSearchResult.SubGroupID != newModel.SubGroupID)
            {
                newModel.ModelState.AddError("CodeExists", "Sub Group Code already exists under a different ID (ID = " + sgCodeSearchResult.SubGroupID.ToString() + "Name: " + sgCodeSearchResult.SubGroupName + ") and must be unique");
                return false;
            }
            //Generic Value Checks
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false || pgIDSearchResult == null || pgIDSearchResult.ProductGroupID <= 0)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //Check something has actually changed
            else if (sgIDSearchResult.SubGroupCode != newModel.SubGroupCode || sgIDSearchResult.ProductGroupID != newModel.ProductGroupID || sgIDSearchResult.SubGroupName != newModel.SubGroupName || sgIDSearchResult.SubGroupDescription != newModel.SubGroupDescription)
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
        private bool ValidateAllValues(SubGroup model)
        {
            if (ValidateID(model.SubGroupID) == false || ValidateID(model.ProductGroupID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.SubGroupCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.SubGroupCode) || string.IsNullOrEmpty(model.SubGroupName) || string.IsNullOrEmpty(model.SubGroupDescription))
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