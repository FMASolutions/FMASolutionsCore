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
            _postCodeService = service;
        }
        public void Dispose()
        {
            _postCodeService.Dispose();
        }
        private ICustomModelState _modelState;
        private IPostCodeService _postCodeService;
        public ICustomModelState ModelState { get { return _modelState; } }

        public PostCodeViewModel Search(int id = 0, string code = "")
        {
            PostCode searchResult = null;
            if (id > 0)
                searchResult = _postCodeService.GetByID(id);
            if (searchResult == null && !string.IsNullOrEmpty(code))
                searchResult = _postCodeService.GetByCode(code);

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
            List<PostCode> postCodeList = _postCodeService.GetAll();
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

        public Dictionary<int, string> GetAvailableCities()
        {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            var list = _postCodeService.GetAvailableCities();
            if (list != null)
                foreach (var item in list)
                    cities.Add(item.CityID, item.CityID.ToString() + " (" + item.CityCode + ") - " + item.CityName);
            return cities;
        }

        public PostCodeViewModel Create(PostCodeViewModel newPostCode)
        {
            PostCode postCode = ConvertToModel(newPostCode);
            postCode.PostCodeID = 0; //NOT SURE I NEED TRHIS???????????????
            PostCodeViewModel vmReturn = new PostCodeViewModel();
            if (_postCodeService.CreateNew(postCode))
                vmReturn = ConvertToViewModel(postCode);
            else
            {
                vmReturn.StatusMessage = "Unable to create Postcode";
                foreach (string item in postCode.ModelState.ErrorDictionary.Values)
                    vmReturn.StatusMessage += " " + item;
            }
            return vmReturn;
        }

        public bool UpdateDB(PostCodeViewModel updatedPostCode)
        {
            PostCode postCode = ConvertToModel(updatedPostCode);
            if (_postCodeService.UpdateDB(postCode))
                return true;
            else
                _modelState = postCode.ModelState;
            return false;
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