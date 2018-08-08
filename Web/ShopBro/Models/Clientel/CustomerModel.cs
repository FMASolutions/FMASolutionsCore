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
            _service = service;
        }
        public void Dispose()
        {
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private ICustomerService _service;

        public ICustomModelState ModelState { get { return _modelState; } }

        public CustomerViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new Customer(_modelState));
        }
        
        public CustomerViewModel Search(int id = 0, string code = "")
        {
            Customer searchResult = null;
            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                CustomerViewModel returnVM = new CustomerViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }

        public CustomersViewModel GetAllCustomers()
        {
            List<Customer> customerList = _service.GetAll();
            CustomersViewModel vmReturn = new CustomersViewModel();

            if (customerList != null && customerList.Count > 0)
            {
                foreach (Customer item in customerList)
                {
                    vmReturn.Customers.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Customers Found";
            return vmReturn;
        }

        public CustomerViewModel Create(CustomerViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            Customer model = ConvertToModel(vmUserInput);
            CustomerViewModel vmResult = ConvertToViewModel(model);

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

        public CustomerViewModel UpdateDB(CustomerViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            Customer model = ConvertToModel(vmUserInput);
            CustomerViewModel vmResult = ConvertToViewModel(model);
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

        private Dictionary<int, string> GetAvailableCustomerTypes()
        {
            Dictionary<int, string> customerTypes = new Dictionary<int, string>();
            var list = _service.GetAvailableCustomerTypes();
            if (list != null)
                foreach (var item in list)
                    customerTypes.Add(item.CustomerTypeID, item.CustomerTypeID.ToString() + " (" + item.CustomerTypeCode + ") - " + item.CustomerTypeName);
            return customerTypes;
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