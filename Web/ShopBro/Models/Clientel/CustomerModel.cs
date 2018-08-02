using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class CustomerModel : IModel, IDisposable
    {
        public CustomerModel(ICustomModelState modelState, ICustomerService service)
        {
            _modelState = modelState;
            _customerService = service;
        }
        public void Dispose()
        {
            _customerService.Dispose();
        }
        private ICustomModelState _modelState;
        private ICustomerService _customerService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public CustomerViewModel Search(int id = 0, string code = "")
        {
            Customer searchResult = null;
            if (id > 0)
                searchResult = _customerService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _customerService.GetByCode(code);

            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CustomerViewModel returnVM = new CustomerViewModel();
                returnVM.StatusErrorMessage = "No result Found";
                return returnVM;
            }
        }

        public CustomersViewModel GetAllCustomers()
        {
            List<Customer> customerList = _customerService.GetAll();
            CustomersViewModel vmReturn = new CustomersViewModel();

            if (customerList != null && customerList.Count > 0)
            {
                foreach (Customer item in customerList)
                {
                    vmReturn.Customers.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusErrorMessage = "No Customers Found";
            return vmReturn;
        }

        public Dictionary<int, string> GetAvailableCustomerTypes()
        {
            Dictionary<int, string> customerTypes = new Dictionary<int, string>();
            var list = _customerService.GetAvailableCustomerTypes();
            if (list != null)   
                foreach (var item in list)
                    customerTypes.Add(item.CustomerTypeID, item.CustomerTypeID.ToString() + " (" + item.CustomerTypeCode + ") - " + item.CustomerTypeName);
            return customerTypes;
        }

        public CustomerViewModel Create(CustomerViewModel newCustomer)
        {
            Customer customer = ConvertToModel(newCustomer);
            customer.CustomerID = 0; //NOT SURE I NEED TRHIS???????????????
            CustomerViewModel vmReturn = new CustomerViewModel();
            if (_customerService.CreateNew(customer))
                vmReturn = ConvertToViewModel(customer);
            else
            {
                vmReturn.StatusErrorMessage = "Unable to create Customer";
                foreach (string item in customer.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusErrorMessage += " " + item;
            }
            return vmReturn;
        }

        public bool UpdateDB(CustomerViewModel updatedCustomer)
        {
            Customer customer = ConvertToModel(updatedCustomer);
            if (_customerService.UpdateDB(customer))
                return true;
            else
                _modelState = customer.ModelState;
            return false;
        }
        private CustomerViewModel ConvertToViewModel(Customer model)
        {
            CustomerViewModel vm = new CustomerViewModel();
            vm.CustomerTypeID = model.CustomerTypeID;
            vm.CustomerID = model.CustomerID;
            vm.CustomerCode = model.CustomerCode;
            vm.CustomerName = model.CustomerName;
            vm.CustomerContactNumber = model.CustomerContactNumber;
            vm.CustomerEmailAddress = model.CustomerEmailAddress;            
            vm.AvailableCustomerTypes = GetAvailableCustomerTypes();
            return vm;
        }

        private Customer ConvertToModel(CustomerViewModel vm)
        {
            Customer customer = new Customer(_modelState
                , vm.CustomerID
                , vm.CustomerCode
                , vm.CustomerTypeID
                , vm.CustomerName
                , vm.CustomerContactNumber
                , vm.CustomerEmailAddress
            );
            return customer;
        }
    }
}