using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class ItemModel : IModel, IDisposable
    {
        public ItemModel(ICustomModelState modelState, IItemService itemService)
        {
            _modelState = modelState;
            _service = itemService;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private IItemService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        
        public ItemViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new Item(_modelState));
        }
        public ItemViewModel Search(int id = 0, string code = "")
        {
            Item searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && string.IsNullOrEmpty(code) == false)
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                ItemViewModel returnVM = new ItemViewModel();
                returnVM.StatusMessage = "No Result Found";
                return returnVM;
            }
        }

        public ItemsViewModel GetAllItems()
        {
            ItemsViewModel vmReturn = new ItemsViewModel();
            List<Item> itemsList = _service.GetAll();
            if (itemsList != null && itemsList.Count > 0)
            {
                foreach (var item in itemsList)
                {
                    vmReturn.Items.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Items Found";
            return vmReturn;
        }        

        public ItemViewModel Create(ItemViewModel vmUserInput, IFormFile uploadFile, IHostingEnvironment he)
        {            
            UploadAndUpdateImage(vmUserInput, uploadFile, he);
            _modelState.ErrorDictionary.Clear();

            Item model = ConvertToModel(vmUserInput);
            ItemViewModel vmResult = ConvertToViewModel(model);

            _modelState = model.ModelState;

            if (_service.CreateNew(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Create Complete.";
            }
            else
            {
                vmUserInput.StatusMessage = "Create Failed: ";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;

            }
            return vmResult;
        }

        public ItemViewModel UpdateDB(ItemViewModel vmUserInput, IFormFile uploadFile, IHostingEnvironment he)
        {
            UploadAndUpdateImage(vmUserInput, uploadFile, he);
            _modelState.ErrorDictionary.Clear();

            Item model = ConvertToModel(vmUserInput);
            ItemViewModel vmResult = ConvertToViewModel(model);
            if (_service.UpdateDB(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Update Complete.";
            }
            else
            {//Return 
                vmResult.StatusMessage = "Update Failed: ";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
        }

        private Dictionary<int, string> GetAvailableSubGroups()
        {
            Dictionary<int, string> subGroups = null;

            var list = _service.GetAllAvailableSubGroups();
            if (list != null)
            {
                subGroups = new Dictionary<int, string>();
                foreach (var item in list)
                    subGroups.Add(item.SubGroupID, item.SubGroupID.ToString() + " (" + item.SubGroupCode + ") - " + item.SubGroupName);
            }
            return subGroups;
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
            Item item = new Item(_modelState
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
            if (uploadFile != null && uploadFile.FileName.Length > 0)
            {
                var uploads = System.IO.Path.Combine(he.WebRootPath, "SiteAssets/images/Items");
                System.IO.Directory.CreateDirectory(uploads);
                var filePath = System.IO.Path.Combine(uploads, newModel.ItemCode + uploadFile.FileName);
                
                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    uploadFile.CopyTo(stream);
                }
                newModel.ItemImageFilename = newModel.ItemCode + uploadFile.FileName;
            }
            else if (string.IsNullOrEmpty(newModel.ItemImageFilename) && uploadFile == null)
                newModel.ItemImageFilename = "Default.png";
        }
    }
}