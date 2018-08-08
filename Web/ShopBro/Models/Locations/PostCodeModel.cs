using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class PostCodeModel : IModel, IDisposable
    {
        public PostCodeModel(ICustomModelState modelState, IPostCodeService service)
        {
            _modelState = modelState;
            _service = service;
        }
        public void Dispose()
        {
            _service.Dispose();
        }
        private ICustomModelState _modelState;
        private IPostCodeService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public PostCodeViewModel GetEmptyViewModel()
        {
            return ConvertToViewModel(new PostCode(_modelState));
        }

        public PostCodeViewModel Search(int id = 0, string code = "")
        {
            PostCode searchResult = null;
            if (id > 0)
                searchResult = _service.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _service.GetByCode(code);
            if (searchResult != null)
                return ConvertToViewModel(searchResult);
            else
            {
                PostCodeViewModel returnVM = new PostCodeViewModel();
                returnVM.StatusMessage = "No result Found";
                return returnVM;
            }
        }

        public PostCodesViewModel GetAllPostCodes()
        {
            List<PostCode> postCodeList = _service.GetAll();
            PostCodesViewModel vmReturn = new PostCodesViewModel();

            if (postCodeList != null && postCodeList.Count > 0)
            {
                foreach (PostCode item in postCodeList)
                {
                    vmReturn.PostCodes.Add(ConvertToViewModel(item));
                }
            }
            else
                vmReturn.StatusMessage = "No Postcodes Found";
            return vmReturn;
        }

        public PostCodeViewModel Create(PostCodeViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            PostCode model = ConvertToModel(vmUserInput);
            PostCodeViewModel vmResult = ConvertToViewModel(model);

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

        public PostCodeViewModel UpdateDB(PostCodeViewModel vmUserInput)
        {
            _modelState.ErrorDictionary.Clear();

            PostCode model = ConvertToModel(vmUserInput);
            PostCodeViewModel vmResult = ConvertToViewModel(model);
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

        private Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _service.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }

        private PostCodeViewModel ConvertToViewModel(PostCode model)
        {
            PostCodeViewModel vm = new PostCodeViewModel();
            vm.CityID = model.CityID;
            vm.PostCodeID = model.PostCodeID;
            vm.PostCodeCode = model.PostCodeCode;
            vm.PostCodeValue = model.PostCodeValue;
            vm.AvailableCities = GetAvailableCities();
            return vm;
        }

        private PostCode ConvertToModel(PostCodeViewModel vm)
        {
            PostCode postCode = new PostCode(_modelState
                , vm.PostCodeID
                , vm.PostCodeCode
                , vm.CityID
                , vm.PostCodeValue
            );
            return postCode;
        }
    }
}