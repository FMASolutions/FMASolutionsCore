using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService.SubGroups;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class SubGroupModel : IModel
    {
        public SubGroupModel(ICustomModelState modelState, ISubGroupService service)
        {
            _modelState = modelState;
            _subGroupService = service;
        }
        private ICustomModelState _modelState;
        private ISubGroupService _subGroupService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public SubGroupViewModel Search(int id = 0, string code = "")
        {
            SubGroup searchResult = null;
            if (id > 0)
                searchResult = _subGroupService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _subGroupService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                SubGroupViewModel returnVM = new SubGroupViewModel();
                returnVM.StatusErrorMessage = "No result Found";
                return returnVM;
            }
        }

        public SubGroupsViewModel GetAllSubGroups()
        {
            List<SubGroup> subGroupList = _subGroupService.GetAll();
            SubGroupsViewModel vmReturn = new SubGroupsViewModel();

            if (subGroupList != null && subGroupList.Count > 0)
            {
                foreach (SubGroup item in subGroupList)
                {
                    vmReturn.SubGroups.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No Sub Groups Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableProductGroups()
        {
            Dictionary<int, string> productGroups = new Dictionary<int, string>();
            var list = _subGroupService.GetAvailableProductGroups();
            if (list != null)
                foreach (var item in list)
                    productGroups.Add(item.ProductGroupID, item.ProductGroupID.ToString() + " (" + item.ProductGroupCode + ") - " + item.ProductGroupName);
            return productGroups;
        }

        public SubGroupViewModel Create(SubGroupViewModel newSubGroup)
        {
            SubGroup subGroup = ConvertToModel(newSubGroup);
            subGroup.SubGroupID = 0; //NOT SURE I NEED TRHIS???????????????
            SubGroupViewModel vmReturn = new SubGroupViewModel();
            if (_subGroupService.CreateNew(subGroup))
                vmReturn = ConvertToViewModel(subGroup);
            else
            {
                vmReturn.StatusErrorMessage = "Unable to create Sub Group";
                foreach (string item in subGroup.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusErrorMessage += " " + item;
            }
            return vmReturn;
        }

        public bool UpdateDB(SubGroupViewModel updatedSubGroup)
        {
            SubGroup subGroup = ConvertToModel(updatedSubGroup);
            if (_subGroupService.UpdateDB(subGroup))
                return true;
            else
                _modelState = subGroup.ModelState;
            return false;
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