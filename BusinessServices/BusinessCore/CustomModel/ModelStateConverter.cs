using FMASolutionsCore.BusinessServices.ControllerTemplate;
namespace FMASolutionsCore.BusinessServices.BusinessCore.CustomModel
{
    public class ModelStateConverter : IModelStateConverter
    {
        public ModelStateConverter(IController controller)
        {
            _controller = controller;
        }
        private IController _controller;
        public ICustomModelState Convert()
        {
            return Convert(_controller);
        }
        private ICustomModelState Convert(IController controller)
        {
            ICustomModelState returnModel = new CustomModelState();

            foreach (var modelStateKey in controller.ModelState.Keys)
            {
                var modelStateVal = controller.ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    returnModel.AddError(modelStateKey, error.ErrorMessage);
                }
            }
            return returnModel;
        }
    }
}