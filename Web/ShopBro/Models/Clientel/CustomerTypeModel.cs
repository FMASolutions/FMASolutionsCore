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
            _customerTypeService = customerTypeService;
        }
        public void Dispose()
        {
            _customerTypeService.Dispose();
        }

        public ICustomModelState ModelState { get { return _modelState; } set { _modelState = value; } }
        private ICustomModelState _modelState;
        private ICustomerTypeService _customerTypeService;

        public CustomerTypeViewModel Search(int id = 0, string code = "")
        {
            CustomerType searchResult = null;

            if (id > 0)
                searchResult = _customerTypeService.GetByID(id);
            if (!string.IsNullOrEmpty(code) && searchResult == null)
                searchResult = _customerTypeService.GetByCode(code);
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
            List<CustomerType> customerTypeList = _customerTypeService.GetAll();
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

        public CustomerTypeViewModel Create(CustomerTypeViewModel newCustomerType)
        {
            CustomerType customerType = ConvertToModel(newCustomerType);
            customerType.CustomerTypeID = 0; //NOT SURE I NEED THIS?????????????
            CustomerTypeViewModel vmReturn = new CustomerTypeViewModel();
            if (_customerTypeService.CreateNew(customerType))
                vmReturn = ConvertToViewModel(customerType);
            else
            {
                vmReturn.StatusMessage = "Unable to create Customer Type";
                foreach (string item in customerType.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusMessage += item + " ";
            }
            return vmReturn;
        }

        public bool UpdateDB(CustomerTypeViewModel updatedCustomerType)
        {
            CustomerType customerType = ConvertToModel(updatedCustomerType);
            if (_customerTypeService.UpdateDB(customerType))
                return true;
            else
                _modelState = customerType.ModelState;
            return false;
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