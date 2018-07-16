using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService.SubGroups;
namespace FMASolutionsCore.BusinessServices.ShoppingService.Items
{
    public class ItemService : IItemService
    {
        public ItemService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
            _subGroupSerice = new SubGroupService(connectionString, dbType);
        }

        private IUnitOfWork _uow;
        private ISubGroupService _subGroupSerice;

        #region IItemService
        public Item GetByID(int id)
        {
            try
            {
                ItemEntity entity = _uow.ItemRepository.GetByID(id);
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

        public Item GetByCode(string code)
        {
            try
            {
                ItemEntity entity = _uow.ItemRepository.GetByCode(code);
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

        public bool CreateNew(Item model)
        {
            try
            {
                bool success = false;
                if (ValidateForCreate(model))
                {
                    model.ItemID = _uow.ItemRepository.GetNextAvailableID();
                    ItemEntity entity = ConvertModelToEntity(model);
                    success = _uow.ItemRepository.Create(entity);
                    if (success)
                    {
                        model.ItemID = entity.ItemID;
                        _uow.SaveChanges();
                    }
                    else
                        model.ModelState.AddError("CreateFailed", "Unable to create Item");
                }
                return success;
            }
            catch (Exception ex)
            {
                model.ModelState.AddError(ex.InnerException.GetType().ToString(), ex.Message);
                return false;
            }
        }

        public List<Item> GetAll()
        {
            List<Item> returnList = null;
            var initialList = _uow.ItemRepository.GetAll();
            if (initialList != null)
            {
                returnList = new List<Item>();
                foreach (var item in initialList)
                    returnList.Add(new Item(new CustomModelState()
                        , item.ItemID
                        , item.ItemCode
                        , item.SubGroupID
                        , item.ItemName
                        , item.ItemDescription
                        , item.ItemUnitPrice
                        , item.ItemUnitPriceWithMaxDiscount
                        , item.ItemAvailableQty
                        , item.ItemReorderQtyReminder
                        , item.ItemImageFilename
                    ));
            }
            return returnList;

        }

        public List<SubGroup> GetAllAvailableSubGroups()
        {
            return _subGroupSerice.GetAll();
        }

        public bool UpdateDB(Item newModel)
        {
            bool updateSuccess = false;
            if (ValidateForUpdate(newModel))
            {
                updateSuccess = _uow.ItemRepository.Update(newModel);
                _uow.SaveChanges();
            }
            return updateSuccess;

        }
        #endregion

        #region Private functions
        private Item ConvertEntityToModel(ItemEntity entity)
        {
            return new Item(new CustomModelState()
                , entity.ItemID
                , entity.ItemCode
                , entity.SubGroupID
                , entity.ItemName
                , entity.ItemDescription
                , entity.ItemUnitPrice
                , entity.ItemUnitPriceWithMaxDiscount
                , entity.ItemAvailableQty
                , entity.ItemReorderQtyReminder
                , entity.ItemImageFilename
            );
        }
        private ItemEntity ConvertModelToEntity(Item model)
        {
            return new ItemEntity(model.ItemID
                , model.SubGroupID
                , model.ItemCode
                , model.ItemName
                , model.ItemDescription
                , model.ItemUnitPrice
                , model.ItemUnitPriceWithMaxDiscount
                , model.ItemAvailableQty
                , model.ItemReorderQtyReminder
                , model.ItemImageFilename
            );
        }

        private bool ValidateForCreate(Item model)
        {
            if (model.ModelState.IsValid)
            {
                if (model.ItemCode.Length > 5)
                {
                    model.ModelState.AddError("CodeLength", "Code should not be greater than 5 characters");
                    return false;
                }
                else if (string.IsNullOrEmpty(model.ItemCode) || model.SubGroupID <= 0 || string.IsNullOrEmpty(model.ItemName) || string.IsNullOrEmpty(model.ItemDescription) || model.ItemUnitPrice <= 0 || model.ItemUnitPriceWithMaxDiscount <= 0 || model.ItemAvailableQty <= 0 || model.ItemReorderQtyReminder <= 0 || string.IsNullOrEmpty(model.ItemImageFilename))
                {
                    model.ModelState.AddError("MissingValue", "One or more values are missing");
                    return false;
                }
                else if (CodeExists(model.ItemCode))
                {
                    model.ModelState.AddError("CodeExists", "Provided code already exists and must be unique");
                    return false;
                }
                else if (SubGroupIDExists(model.SubGroupID) == false)
                {
                    model.ModelState.AddError("SubGroupID", "Sub Group ID Provided does not exists");
                    return false;
                }
            }
            else
                return false;
            return true;
        }
        private bool ValidateForUpdate(Item model)
        {
            ItemEntity itemIDSearchResult = _uow.ItemRepository.GetByID(model.ItemID);
            ItemEntity itemCodeSearchResult = _uow.ItemRepository.GetByCode(model.ItemCode);
            SubGroupEntity subIDSearchResult = _uow.SubGroupRepository.GetByID(model.SubGroupID);
            if (itemCodeSearchResult != null && itemCodeSearchResult.ItemID != model.ItemID)
            {
                model.ModelState.AddError("CodeExists", "Item Code already exists under a different ID (ID = " + itemCodeSearchResult.ItemID.ToString() + "Name: " + itemCodeSearchResult.ItemName + ") and must be unique");
                return false;
            }
            else if (ValidateAllValues(model) == false || model.ModelState.IsValid == false || subIDSearchResult == null || subIDSearchResult.SubGroupID <= 0)
            {
                model.ModelState.AddError("InvalidValues", "One or more values were invalid");
                return false;
            }
            else if (DetectValueChange(model, itemIDSearchResult))
                return true;
            else
            {
                return false;
            }
        }

        private bool ValidateAllValues(Item model)
        {
            if (ValidateID(model.ItemID) == false || ValidateID(model.SubGroupID) == false)
            {
                model.ModelState.AddError("InvalidID", "ID value was invalid");
                return false;
            }
            else if (ValidateCode(model.ItemCode) == false)
            {
                model.ModelState.AddError("InvalidCode", "Code value was invalid, it can't be more than 5 characters or empty");
                return false;
            }
            else if (string.IsNullOrEmpty(model.ItemCode) || model.SubGroupID <= 0 || string.IsNullOrEmpty(model.ItemName) || string.IsNullOrEmpty(model.ItemDescription) || model.ItemUnitPrice <= 0 || model.ItemUnitPriceWithMaxDiscount <= 0 || model.ItemAvailableQty <= 0 || model.ItemReorderQtyReminder <= 0 || string.IsNullOrEmpty(model.ItemImageFilename))
            {
                model.ModelState.AddError("Null", "Values cant be blank.");
                return false;
            }
            else
                return true;
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
        private bool DetectValueChange(Item model, ItemEntity entity)
        {
            if (entity.ItemCode != model.ItemCode
                || entity.SubGroupID != model.SubGroupID
                || entity.ItemName != model.ItemName
                || entity.ItemDescription != model.ItemDescription
                || entity.ItemUnitPrice != model.ItemUnitPrice
                || entity.ItemUnitPriceWithMaxDiscount != model.ItemUnitPriceWithMaxDiscount
                || entity.ItemAvailableQty != model.ItemAvailableQty
                || entity.ItemReorderQtyReminder != model.ItemReorderQtyReminder
                || entity.ItemImageFilename != model.ItemImageFilename)
            {
                return true;
            }
            return false;
        }
        private bool CodeExists(string code)
        {
            var result = GetByCode(code);
            if (result != null && result.ItemID > 0)
                return true;
            else
                return false;
        }

        private bool SubGroupIDExists(int id)
        {
            var result = _uow.SubGroupRepository.GetByID(id);
            if (result != null && result.SubGroupID > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}