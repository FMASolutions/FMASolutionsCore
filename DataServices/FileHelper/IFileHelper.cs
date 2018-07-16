namespace FMASolutionsCore.DataServices.FileHelper
{
    public enum EnumFileHelperTypes
    {
        Unknown,
        TextFile,
    }
    public interface IFileHelper
    {
        string FileExtension { get; }

        string FileName { get; }

        string FileLocation { get; }

        string FilePathFull { get; }

        void AppendLineToFile(string lineToWrite);

        string[] GetAllLinesInFile();
        string GetFirstLineInFile();
        void DeleteFile();
    }
}