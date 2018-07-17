using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class ProductGroupService : IProductGroupService
    {
        public ProductGroupService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
        }
        private IUnitOfWork _uow;

        public ProductGroup GetByID(int id)
        {
            try
            {
                ProductGroupEntity entity = _uow.ProductGroupRepo.GetByID(id);
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
        public ProductGroup GetByCode(string code)
        {
            try
            {
                ProductGroupEntity entity = _uow.ProductGroupRepo.GetByCode(code);
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
        public bool CreateNew(ProductGroup model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.ProductGroupID = _uow.ProductGroupRepo.GetNextAvailableID();
                    ProductGroupEntity entity = ConvertModelToEntity(model);
                    success = _uow.ProductGroupRepo.Create(entity);
                    if (success)
                    {
                        model.ProductGroupID = entity.ProductGroupID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create new product group");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.InnerException.GetType().ToString(), ex.Message);
                return false;
            }
        }
        public List<ProductGroup> GetAll()
        {
            List<ProductGroup> returnList = null;
            var initialList = _uow.ProductGroupRepo.GetAll();
            if (initialList != null)
            {
                returnList = new List<ProductGroup>();
                foreach (var item in initialList)
                    returnList.Add(new ProductGroup(new CustomModelState(), item.ProductGroupID, item.ProductGroupCode, item.ProductGroupName, item.ProductGroupDescription));
            }
            return returnList;
        }
        public bool UpdateDB(ProductGroup newModel)
        {
            bool updateSuccess = false;

            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.ProductGroupRepo.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;
        }

        #region Private Functions
        private bool CodeExists(string code)
        {
            ProductGroupEntity ent = _uow.ProductGroupRepo.GetByCode(code);
            if (ent != null && ent.ProductGroupID > 0)
                return true;
            else
                return false;
        }
        private ProductGroup ConvertEntityToModel(ProductGroupEntity entity)
        {
            return new ProductGroup(new CustomModelState()
                , entity.ProductGroupID
                , entity.ProductGroupCode
                , entity.ProductGroupName
                , entity.ProductGroupDescription
            );
        }
        private ProductGroupEntity ConvertModelToEntity(ProductGroup model)
        {
            return new ProductGroupEntity(model.ProductGroupID, model.ProductGroupCode, model.ProductGroupName, model.ProductGroupDescription);
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
        private bool ValidateAllValues(ProductGroup model)
        {
            if (ValidateID(model.ProductGroupID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.ProductGroupCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.ProductGroupCode) || string.IsNullOrEmpty(model.ProductGroupName) || string.IsNullOrEmpty(model.ProductGroupDescription))
            {
                model.ModelState.AddError("Null", "Values cant be blank.");
                return false;
            }
            else return true;
        }
        private bool ValidateForUpdate(ProductGroup newModel)
        {
            ProductGroupEntity entityFromIDSearch = _uow.ProductGroupRepo.GetByID(newModel.ProductGroupID);
            ProductGroupEntity entityFromCodeSearch = _uow.ProductGroupRepo.GetByCode(newModel.ProductGroupCode);
            //Check our new code (if it is even new) doesn't exist under a different ID which would cause a proble with the UNIQUE contraint on the CODE column.                        
            if (entityFromCodeSearch != null && entityFromCodeSearch.ID != newModel.ProductGroupID)
            {
                newModel.ModelState.AddError("CodeExists", @"Product Group Code already exists under a different ID (ID = " + entityFromCodeSearch.ProductGroupID.ToString() + " Name = " + entityFromCodeSearch.ProductGroupName + ") and must be unique");
                return false;
            }
            else if (ValidateAllValues(newModel) == false || newModel.ModelState.IsValid == false)
            {
                newModel.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            //If user changed something, return true at this point
            else if (entityFromIDSearch.ProductGroupName != newModel.ProductGroupName || entityFromIDSearch.ProductGroupDescription != newModel.ProductGroupDescription || entityFromIDSearch.ProductGroupCode != newModel.ProductGroupCode)
                return true;
            else
                newModel.ModelState.AddError("NoChange", "No Changes detected");
            return false;
        }
        private bool ValidateForCreate(ProductGroup model)
        {
            if (model.ModelState.IsValid)
            {
                if (model.ProductGroupCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greather than 5 characters");
                    return false;
                }
                else if (string.IsNullOrEmpty(model.ProductGroupCode) || string.IsNullOrEmpty(model.ProductGroupName) || string.IsNullOrEmpty(model.ProductGroupDescription))
                {
                    model.ModelState.AddError("NullValues", "All values must be populated...");
                    return false;
                }
                else if (CodeExists(model.ProductGroupCode))
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