using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class PostCode : PostCodeEntity, IModel
    {
        public PostCode(ICustomModelState modelState, int postCodeID = 0, string postCodeCode = "", int cityID = 0, string postCodeValue = "")
        {
            this.ModelState = modelState;
            this._postCodeID  = postCodeID;
            this._postCodeCode = postCodeCode;
            this._cityID = cityID;
            this._postCodeValue = postCodeValue;
        }
        public ICustomModelState ModelState { get { return _modelState; } private set { _modelState = value; } }
        private ICustomModelState _modelState;
    }
}