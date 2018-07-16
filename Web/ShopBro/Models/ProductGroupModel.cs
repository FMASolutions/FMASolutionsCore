using FMASolutionsCore.BusinessServices.ShoppingService.ProductGroups;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class ProductGroupModel
    {
        public ProductGroupModel(ICustomModelState modelState, IProductGroupService service)
        {
            _productGroupService = service;
            _modelState = modelState;
        }

        public ICustomModelState ModelState { get { return _modelState; } }

        private ICustomModelState _modelState;
        private IProductGroupService _productGroupService;

        public ProductGroupViewModel Search(int id = 0, string code = "")
        {
            ProductGroup searchResult = null;

            if (id > 0)
                searchResult = _productGroupService.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _productGroupService.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                ProductGroupViewModel returnVM = new ProductGroupViewModel();
                returnVM.StatusErrorMessage = "No result found";
                return returnVM;
            }
        }
        public ProductGroupsViewModel GetAllProductGroups()
        {
            List<ProductGroup> productGroupList = _productGroupService.GetAll();
            ProductGroupsViewModel vmReturn = new ProductGroupsViewModel();

            if (productGroupList != null && productGroupList.Count > 0)
            {
                foreach (ProductGroup item in productGroupList)
                {
                    vmReturn.ProductGroups.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No Product Groups Found";
            return vmReturn;
        }

        public ProductGroupViewModel Create(ProductGroupViewModel newProductGroup)
        {
            ProductGroup productGroup = ConvertToModel(newProductGroup);
            productGroup.ProductGroupID = 0; //NOT SURE I NEED THIS?????????????
            ProductGroupViewModel vmReturn = new ProductGroupViewModel();
            if (_productGroupService.CreateNew(productGroup))
                vmReturn = ConvertToViewModel(productGroup);
            else
            {
                vmReturn.StatusErrorMessage = "Unable to create Product Group";
                foreach (string item in productGroup.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusErrorMessage += item + " ";
            }
            return vmReturn;
        }

        public bool UpdateDB(ProductGroupViewModel updatedProductGroup)
        {
            ProductGroup productGroup = ConvertToModel(updatedProductGroup);
            if (_productGroupService.UpdateDB(productGroup))
                return true;
            else
                _modelState = productGroup.ModelState;
            return false;
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