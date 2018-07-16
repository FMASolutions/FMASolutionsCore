using System.Collections.Generic;
namespace FMASolutionsCore.BusinessServices.BusinessCore.CustomModel
{
    public class CustomModelState : ICustomModelState
    {
        public CustomModelState()
        {
            _errorDictionary = new Dictionary<string, string>();
        }
        private Dictionary<string, string> _errorDictionary;
        public Dictionary<string, string> ErrorDictionary { get { return _errorDictionary; } }
        public void AddError(string errorKey, string errorValue)
        {
            _errorDictionary.Add(errorKey, errorValue);
        }
        public bool IsValid
        {
            get
            {
                if (_errorDictionary.Count > 0)
                    return false;
                else
                    return true;
            }
        }
    }
}