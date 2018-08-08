using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class ProductGroupModel : IModel, IDisposable
    {
        public ProductGroupModel(ICustomModelState modelState, IProductGroupService service)
        {
            _service = service;
            _modelState = modelState;
        }
        public void Dispose()
        {
            _service.Dispose();
        }
        
        private ICustomModelState _modelState;
        private IProductGroupService _service;
        public ICustomModelState ModelState { get { return _modelState; } }
        
        public ProductGroupViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new ProductGroup(_modelState));
        }
        public ProductGroupViewModel Search(int id = 0, string code = "")
        {
            ProductGroup searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                ProductGroupViewModel returnVM = new ProductGroupViewModel();
                returnVM.StatusMessage = "No result found";
                return returnVM;
            }
        }
        public ProductGroupsViewModel GetAllProductGroups()
        {
            List<ProductGroup> productGroupList = _service.GetAll();
            ProductGroupsViewModel vmReturn = new ProductGroupsViewModel();

            if (productGroupList != null && productGroupList.Count > 0)
            {
                foreach (ProductGroup item in productGroupList)
                {
                    vmReturn.ProductGroups.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Product Groups Found";
            return vmReturn;
        }

        public ProductGroupViewModel Create(ProductGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            ProductGroup model = ConvertToModel(vmUserInput);
            ProductGroupViewModel vmResult = new ProductGroupViewModel();

            _modelState = model.ModelState;

            if (_service.CreateNew(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Create Complete";
            }
            else
            {
                vmResult.StatusMessage = "Create Failed";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
        }

        public ProductGroupViewModel UpdateDB(ProductGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            ProductGroup model = ConvertToModel(vmUserInput);
            ProductGroupViewModel vmResult = ConvertToViewModel(model);
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

        private ProductGroupViewModel ConvertToViewModel(ProductGroup sourceModel)
        {
            ProductGroupViewModel model = new ProductGroupViewModel();
            model.ProductGroupID = sourceModel.ProductGroupID;
            model.ProductGroupCode = sourceModel.ProductGroupCode;
            model.ProductGroupName = sourceModel.ProductGroupName;
            model.ProductGroupDescription = sourceModel.ProductGroupDescription;
            return model;
        }

        private ProductGroup ConvertToModel(ProductGroupViewModel vm)
        {
            ProductGroup productGroup = new ProductGroup(_modelState
                , vm.ProductGroupID
                , vm.ProductGroupCode
                , vm.ProductGroupName
                , vm.ProductGroupDescription
            );
            return productGroup;
        }
    }
}