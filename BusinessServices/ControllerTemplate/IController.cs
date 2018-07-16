using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace FMASolutionsCore.BusinessServices.ControllerTemplate
{
    public interface IController
    {
        ModelStateDictionary ModelState { get; }
    }
}