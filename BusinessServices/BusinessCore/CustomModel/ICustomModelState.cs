using System.Collections.Generic;
namespace FMASolutionsCore.BusinessServices.BusinessCore.CustomModel
{
    public interface ICustomModelState
    {
        Dictionary<string, string> ErrorDictionary { get; }
        void AddError(string errorKey, string errorValue);
        bool IsValid { get; }
    }
}