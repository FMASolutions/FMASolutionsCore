using System;
using System.IO;

namespace FMASolutionsCore.DataServices.FileHelper
{
    public class TextFileHelper : IFileHelper
    {
        private string _fullFilePath;
        private string _fileExtension;
        private string _fileLocation;
        private string _fileName;

        public TextFileHelper(string fullFilePath)
        {
            if (fullFilePath == string.Empty || fullFilePath == null)
                throw new InvalidDataException(FileHelper.C.InvalidDataErrorText + DateTime.Now.ToString());
            _fullFilePath = fullFilePath;
            int num = _fullFilePath.LastIndexOf('/');
            int startIndex = _fullFilePath.LastIndexOf('.');
            _fileLocation = _fullFilePath.Substring(0, num + 1);
            _fileName = _fullFilePath.Substring(num + 1, startIndex - num - 1);
            _fileExtension = _fullFilePath.Substring(startIndex);
        }

        public string FileExtension { get { return _fileExtension; } }

        public string FileName { get { return _fileName; } }

        public string FileLocation { get { return _fileLocation; } }

        public string FilePathFull { get { return _fullFilePath; } }

        public void AppendLineToFile(string lineToWrite)
        {
            if (!File.Exists(_fullFilePath))
            {
                if (!Directory.Exists(_fileLocation))
                    Directory.CreateDirectory(_fileLocation);

                using (File.Create(_fullFilePath))
                {
                }

                using (StreamWriter text = File.CreateText(_fullFilePath))
                    text.WriteLine(lineToWrite);
            }
            else
                using (StreamWriter streamWriter = File.AppendText(this._fullFilePath))
                    streamWriter.WriteLine(lineToWrite);
        }
        public string[] GetAllLinesInFile()
        {
            return File.ReadAllLines(_fullFilePath);
        }
        public string GetFirstLineInFile()
        {
            return File.ReadAllLines(_fullFilePath)[0];
        }
        public void DeleteFile()
        {
            File.Delete(_fullFilePath);
        }
    }
}
