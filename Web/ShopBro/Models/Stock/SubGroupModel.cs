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
            _subGroupService = service;
        }
        public void Dispose()
        {
            _subGroupService.Dispose();
        }
        private ICustomModelState _modelState;
        private ISubGroupService _subGroupService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public SubGroupViewModel Search(int id = 0, string code = "")
        {
            SubGroup searchResult = null;            
            SubGroupViewModel returnVM;
            if (id > 0)
                searchResult = _subGroupService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _subGroupService.GetByCode(code);

            if (searchResult != null)
            {
                returnVM = ConvertToViewModel(searchResult);
                returnVM.AvailableProductGroups = GetAvailableProductGroups();                
            }
            else
            {
                returnVM = new SubGroupViewModel();
                returnVM.StatusMessage = "No result Found";                
            }
            return returnVM;
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
                vmReturn.StatusMessage = "No Sub Groups Found";
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

        public SubGroupViewModel Create(SubGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();  
            
            SubGroup model = ConvertToModel(vmUserInput);
            SubGroupViewModel vmResult = ConvertToViewModel(model);
            
            _modelState = model.ModelState;

            if (_subGroupService.CreateNew(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Create Complete.";       
            }
            else
            {
                vmUserInput.StatusMessage = "Create Failed: ";
                foreach(string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;                
                        
            }
            return vmResult;   
        }

        public SubGroupViewModel UpdateDB(SubGroupViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();
            
            SubGroup model = ConvertToModel(vmUserInput);
            SubGroupViewModel vmResult =ConvertToViewModel(model);         
            if (_subGroupService.UpdateDB(model))
            {
                vmResult = ConvertToViewModel(model);
                vmResult.StatusMessage = "Update Complete.";
            }
            else
            {//Return 
                vmResult.StatusMessage = "Update Failed: ";
                foreach(string item in model.ModelState.ErrorDictionary.Values)
                    vmResult.StatusMessage += Environment.NewLine + item;
            }
            return vmResult;
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