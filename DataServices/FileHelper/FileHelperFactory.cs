namespace FMASolutionsCore.DataServices.FileHelper
{
    public abstract class FileHelperFactory
    {
        public abstract IFileHelper CreateProduct(EnumFileHelperTypes fileHelperType, string pathToFile);
    }
}