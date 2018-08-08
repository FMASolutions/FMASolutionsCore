using System;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CustomerTypeModel : IModel, IDisposable
    {
        public CustomerTypeModel(ICustomModelState modelState, ICustomerTypeService customerTypeService)
        {
            _modelState = modelState;
            _service = customerTypeService;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private ICustomerTypeService _service;

        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }

        public CustomerTypeViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new CustomerType(_modelState));
        }

        public CustomerTypeViewModel Search(int id = 0, string code = "")
        {
            CustomerType searchResult = null;

            if (id > 0)
                searchResult = _service.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CustomerTypeViewModel returnVM = new CustomerTypeViewModel();
                returnVM.StatusMessage = "No result found";
                return returnVM;
            }
        }
        public CustomerTypesViewModel GetAllCustomerTypes()
        {
            List<CustomerType> customerTypeList = _service.GetAll();
            CustomerTypesViewModel vmReturn = new CustomerTypesViewModel();

            if (customerTypeList != null && customerTypeList.Count > 0)
            {
                foreach (CustomerType item in customerTypeList)
                {
                    vmReturn.CustomerTypes.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Product Groups Found";
            return vmReturn;
        }

        public CustomerTypeViewModel Create(CustomerTypeViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            CustomerType model = ConvertToModel(vmUserInput);
            CustomerTypeViewModel vmResult = ConvertToViewModel(model);

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

        public CustomerTypeViewModel UpdateDB(CustomerTypeViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            CustomerType model = ConvertToModel(vmUserInput);
            CustomerTypeViewModel vmResult = ConvertToViewModel(model);
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

        private CustomerTypeViewModel ConvertToViewModel(CustomerType sourceModel)
        {
            CustomerTypeViewModel model = new CustomerTypeViewModel();
            model.CustomerTypeID = sourceModel.CustomerTypeID;
            model.CustomerTypeCode = sourceModel.CustomerTypeCode;
            model.CustomerTypeName = sourceModel.CustomerTypeName;
            return model;
        }

        private CustomerType ConvertToModel(CustomerTypeViewModel vm)
        {
            CustomerType customerType = new CustomerType(_modelState
                , vm.CustomerTypeID
                , vm.CustomerTypeCode
                , vm.CustomerTypeName
            );
            return customerType;
        }
    }
}