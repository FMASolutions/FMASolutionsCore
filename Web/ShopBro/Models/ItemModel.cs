using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService.Items;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class ItemModel
    {
        public ItemModel(ICustomModelState modelState, IItemService itemService)
        {
            _modelState = modelState;
            _itemService = itemService;
        }

        private ICustomModelState _modelState;
        private IItemService _itemService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public ItemViewModel Search(int id = 0, string code = "")
        {
            Item searchResult = null;
            if (id > 0)
                searchResult = _itemService.GetByID(id);
            if (searchResult == null && string.IsNullOrEmpty(code) == false)
                searchResult = _itemService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                ItemViewModel returnVM = new ItemViewModel();
                returnVM.StatusErrorMessage = "No Result Found";
                return returnVM;
            }
        }

        public ItemsViewModel GetAllItems()
        {
            ItemsViewModel vmReturn = new ItemsViewModel();
            List<Item> itemsList = _itemService.GetAll();
            if (itemsList != null && itemsList.Count > 0)
            {
                foreach (var item in itemsList)
                {
                    vmReturn.Items.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No Items Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableSubGroups()
        {
            Dictionary<int, string> subGroups = null;

            var list = _itemService.GetAllAvailableSubGroups();
            if (list != null)
            {
                subGroups = new Dictionary<int, string>();
                foreach (var item in list)
                    subGroups.Add(item.SubGroupID, item.SubGroupID.ToString() + " (" + item.SubGroupCode + ") - " + item.SubGroupName);
            }
            return subGroups;
        }

        public ItemViewModel Create(ItemViewModel newModel, IFormFile uploadFile, IHostingEnvironment he)
        {
            UploadAndUpdateImage(newModel, uploadFile, he);
            Item item = ConvertToModel(newModel);
            item.ItemID = 0; //NOT 100% SURE I NEED THIS????????????
            ItemViewModel vmReturn = new ItemViewModel();

            if (_itemService.CreateNew(item))
                vmReturn = ConvertToViewModel(item);
            else
            {
                newModel.StatusErrorMessage = "Unable to create subgroup";
                foreach (string message in item.ModelState.ErrorDictionary.Values)
                    newModel.StatusErrorMessage += " " + message;
            }
            return vmReturn;
        }

        public bool UpdateDB(ItemViewModel updatedItem, IFormFile uploadFile, IHostingEnvironment he)
        {
            UploadAndUpdateImage(updatedItem, uploadFile, he);
            Item item = ConvertToModel(updatedItem);
            if (_itemService.UpdateDB(item))
                return true;
            else
                _modelState = item.ModelState;
            return false;
        }

        private ItemViewModel ConvertToViewModel(Item model)
        {
            ItemViewModel returnVM = new ItemViewModel();
            returnVM.ItemID = model.ItemID;
            returnVM.ItemCode = model.ItemCode;
            returnVM.SubGroupID = model.SubGroupID;
            returnVM.ItemName = model.ItemName;
            returnVM.ItemDescription = model.ItemDescription;
            returnVM.ItemUnitPrice = model.ItemUnitPrice;
            returnVM.ItemUnitPriceWithMaxDiscount = model.ItemUnitPriceWithMaxDiscount;
            returnVM.ItemAvailableQty = model.ItemAvailableQty;
            returnVM.ItemReorderQtyReminder = model.ItemReorderQtyReminder;
            returnVM.ItemImageFilename = model.ItemImageFilename;
            returnVM.AvailableSubGroups = GetAvailableSubGroups();
            return returnVM;
        }

        private Item ConvertToModel(ItemViewModel vm)
        {
            Item item = new Item(new CustomModelState()
                , vm.ItemID
                , vm.ItemCode
                , vm.SubGroupID
                , vm.ItemName
                , vm.ItemDescription
                , vm.ItemUnitPrice
                , vm.ItemUnitPriceWithMaxDiscount
                , vm.ItemAvailableQty
                , vm.ItemReorderQtyReminder
                , vm.ItemImageFilename
            );
            return item;
        }

        private void UploadAndUpdateImage(ItemViewModel newModel, IFormFile uploadFile, IHostingEnvironment he)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var uploads = System.IO.Path.Combine(he.WebRootPath, "SiteAssets/images/Items");
                var filePath = System.IO.Path.Combine(uploads, uploadFile.FileName);
                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    uploadFile.CopyTo(stream);
                }
                newModel.ItemImageFilename = uploadFile.FileName;
            }
            else if (string.IsNullOrEmpty(newModel.ItemImageFilename) && uploadFile == null)
                newModel.ItemImageFilename = "Default.png";
        }
    }
}