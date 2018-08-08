using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class SubGroupModel : IModel, IDisposable
    {
        public SubGroupModel(ICustomModelState modelState, ISubGroupService service)
        {
            _modelState = modelState;
            _service = service;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private ISubGroupService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public SubGroupViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new SubGroup(_modelState));
        }
        public SubGroupViewModel Search(int id = 0, string code = "")
        {
            SubGroup searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                SubGroupViewModel returnVM = new SubGroupViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }
        public SubGroupsViewModel GetAllSubGroups()
        {
            List<SubGroup> subGroupList = _service.GetAll();
            SubGroupsViewModel vmReturn = new SubGroupsViewModel();

            if (subGroupList != null && subGroupList.Count > 0)
            {
                foreach (SubGroup item in subGroupList)
                {
                    vmReturn.SubGroups.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Sub Groups Found";
            return vmReturn;
        }

        public SubGroupViewModel Create(SubGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            SubGroup model = ConvertToModel(vmUserInput);
            SubGroupViewModel vmResult = ConvertToViewModel(model);

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

        public SubGroupViewModel UpdateDB(SubGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            SubGroup model = ConvertToModel(vmUserInput);
            SubGroupViewModel vmResult = ConvertToViewModel(model);
            if (_service.UpdateDB(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Update Complete.";
            }
            else
            {
                vmResult.StatusMessage = "Update Failed: ";
                foreach (string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
        }

        private Dictionary<int, string> GetAvailableProductGroups()
        {
            Dictionary<int, string> productGroups = new Dictionary<int, string>();
            var list = _service.GetAvailableProductGroups();
            if (list != null)
                foreach (var item in list)
                    productGroups.Add(item.ProductGroupID, item.ProductGroupID.ToString() + " (" + item.ProductGroupCode + ") - " + item.ProductGroupName);
            return productGroups;
        }
        private SubGroupViewModel ConvertToViewModel(SubGroup model)
        {
            SubGroupViewModel vm = new SubGroupViewModel();
            vm.ProductGroupID = model.ProductGroupID;
            vm.SubGroupID = model.SubGroupID;
            vm.SubGroupCode = model.SubGroupCode;
            vm.SubGroupName = model.SubGroupName;
            vm.SubGroupDescription = model.SubGroupDescription;
            vm.AvailableProductGroups = GetAvailableProductGroups();
            return vm;
        }

        private SubGroup ConvertToModel(SubGroupViewModel vm)
        {
            SubGroup subGroup = new SubGroup(_modelState
                , vm.SubGroupID
                , vm.SubGroupCode
                , vm.ProductGroupID
                , vm.SubGroupName
                , vm.SubGroupDescription
            );
            return subGroup;
        }
    }
}