using FMASolutionsCore.DataServices.FileHelper;
using FMASolutionsCore.BusinessServices.BusinessCore.Generic;

namespace FMASolutionsCore.BusinessServices.AppLoggerExtension
{
    public class AppLoggerExtension : IAppLoggerExtension
    {
        public AppLoggerExtension()
        {

        }

        private FileHelperFactory _factory = (FileHelperFactory)new FileHelperFactoryWorker();
        private IFileHelper _fileHelper;


        public void Register()
        {
            InitializeLogs();
        }

        public void WriteToErrorLog(string message, object sender)
        {
            WriteLineToFile(GetLogLocationWithFilename(Helper.GetCurrentAppName(), C.ErrorLogFileName)
                , GetFormattedMessage(
                    logLinePrefix: C.ErrorLogFileName
                    , messagePrefix: GetMessagePrefixdecoration(C.ErrorLogFileName)
                    , message: message + C.ErrorSenderPrefix + sender.ToString()
            ));
        }

        public void WriteToProcessLog(string message)
        {
            WriteLineToFile(GetLogLocationWithFilename(Helper.GetCurrentAppName(), C.ProcessLogFileName)
                , GetFormattedMessage(
                    logLinePrefix: C.ProcessLogFileName
                    , messagePrefix: GetMessagePrefixdecoration(C.ProcessLogFileName)
                    , message: message
            ));
        }

        public void WriteToUserRequestLog(string message)
        {
            WriteLineToFile(GetLogLocationWithFilename(Helper.GetCurrentAppName(), C.UserRequestLogFileName),
                GetFormattedMessage(
                    logLinePrefix: C.UserRequestLogFileName
                    , messagePrefix: GetMessagePrefixdecoration(C.UserRequestLogFileName)
                    , message: message
            ));
        }

        public void WriteToCustomLog(string filename, string message)
        {
            WriteLineToFile(GetLogLocationWithFilename(Helper.GetCurrentAppName(), filename),
                GetFormattedMessage(
                    logLinePrefix: filename
                    , messagePrefix: GetMessagePrefixdecoration(filename)
                    , message: message
                ));
        }

        private void WriteLineToFile(string fileLocation, string line)
        {
            _fileHelper = _factory.CreateProduct(EnumFileHelperTypes.TextFile, fileLocation);
            _fileHelper.AppendLineToFile(line);
        }

        private string GetFormattedMessage(string logLinePrefix, string messagePrefix, string message, string messageSuffix = "")
        {
            return logLinePrefix + C.LineBeginSuffix + System.DateTime.Now.ToString() + messagePrefix + message + messageSuffix;
        }

        private string GetLogLocationWithFilename(string applicationName, string fileName)
        {
            return C.DefaultLogsFolder + applicationName + "/" + fileName + C.DefaultLogFileExtension;
        }

        private string GetMessagePrefixdecoration(string logFileName)
        {
            return C.MessageDecorationPrefix + logFileName + " " + C.MessageDecorationSuffix;
        }

        //Custom Log Files must only specify the name of the file. the folder and extension are added on automatically
        private void InitializeLogs()
        {
            WriteToErrorLog("Log Initialized", this);
            WriteToUserRequestLog("Log Initialized");
            WriteToProcessLog("Log Initialized");
        }
    }
}