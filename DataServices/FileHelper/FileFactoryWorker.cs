using System;
namespace FMASolutionsCore.DataServices.FileHelper
{
    public class FileHelperFactoryWorker : FileHelperFactory
    {
        public override IFileHelper CreateProduct(EnumFileHelperTypes fileHelperType, string pathToFile)
        {
            if (fileHelperType == EnumFileHelperTypes.TextFile)
                return (IFileHelper)new TextFileHelper(pathToFile);
            throw new ArgumentOutOfRangeException(fileHelperType.ToString());
        }
    }
}